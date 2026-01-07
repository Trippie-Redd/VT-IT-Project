using System;
using System.Collections.Generic;
using System.Threading;

namespace StateMachine
{
    public class TransitionSequencer
    {
        public readonly StateMachine Machine;

        private ISequence _sequencer;              // current phase (deactivate or activate)
        private Action _nextPhase;                 // switch structure between phases
        private (State from, State to)? _pending;  // coalesce a single pending request
        private State _lastFrom, _lastTo; 

        public TransitionSequencer(StateMachine machine)
            => Machine = machine;

        /// <summary>
        /// Request a transition form one state to another
        /// </summary>
        public void RequestTransition(State from, State to)
        {
            if (from == null || to == null) return;
            if (_sequencer != null)
            {
                _pending = (from, to);
                return;
            }
            _BeginTransition(from, to);
        }

        private static List<PhaseStep> _GatherPhaseSteps(List<State> chain, bool deactivate)
        {
            var steps = new List<PhaseStep>();
            foreach (var state in chain)
            {
                IReadOnlyList<IActivity> activities = state.Activities;

                foreach (var activity in activities)
                {
                    if (deactivate)
                    {
                        if (activity.Mode != ActivityMode.Active) continue;
                        
                        var activity1 = activity;
                        steps.Add(ct => activity1.ActivateAsync(ct));
                    }
                    else if (activity.Mode == ActivityMode.Inactive)
                    {
                        steps.Add(ct => activity.ActivateAsync(ct));
                    }
                }
            }

            return steps;
        }

        /// <summary>
        /// From -> ... up to (but excluding) lca; bottom->up order
        /// </summary>
        private static List<State> _StatesToExit(State from, State lca)
        {
            var list = new List<State>();
            for (State state = from; state != null && state != lca; state = state.Parent)
                list.Add(state);

            return list;
        }

        /// <summary>
        /// Path from 'to' up to (but excluding) lca; returned in enter order (top->down)
        /// </summary>
        /// <param name="to"></param>
        /// <param name="lca"></param>
        /// <returns></returns>
        private static List<State> _StatesToEnter(State to, State lca)
        {
            var stack = new Stack<State>();
            for (State state = to; state != lca; state = state.Parent)
                stack.Push(state);

            return new List<State>(stack);
        }

        private CancellationTokenSource cts;
        public readonly bool UseSequential = true; // set false to use parallel

        private void _BeginTransition(State from, State to)
        {
            State lca              = Lca(from, to);
            List<State> exitChain  = _StatesToExit(from, lca);
            List<State> enterChain = _StatesToExit(to, lca);
            
            // 1. Deactivate the "old branch"
            List<PhaseStep> exitSteps = _GatherPhaseSteps(exitChain, deactivate: true);
            _sequencer = UseSequential
                ? new SequentialPhase(exitSteps, cts.Token)
                : new ParallelPhase(exitSteps, cts.Token);
            _sequencer.Start();

            _nextPhase = () =>
            {
                // 2. Change state
                Machine.ChangeState(from, to);
                
                // 3. Activate the "new branch"
                List<PhaseStep> enterSteps = _GatherPhaseSteps(enterChain, deactivate: false);
                _sequencer = UseSequential
                    ? new SequentialPhase(enterSteps, cts.Token)
                    : new ParallelPhase(enterSteps, cts.Token);
                _sequencer.Start();
            };
        }

        private void _EndTransition()
        {
            _sequencer = null;

            if (!_pending.HasValue) return;
            
            var pending = _pending.Value;
            _pending = null;
            _BeginTransition(pending.from, pending.to);
        }

        public void Tick(float deltaTime)
        {
            if (_sequencer != null)
            {
                if (!_sequencer.Update()) return;
                
                if (_nextPhase != null)
                {
                    var nextPhase = _nextPhase;
                    _nextPhase = null;
                    nextPhase();
                }
                else
                {
                    _EndTransition();
                }

                return; // we don't run normal updates whhile transitioning
            }
            
            Machine.InternalTick(deltaTime);
        }

        /// <summary>
        /// Compute the lowest common ancestor of two states
        /// </summary>
        public static State Lca(State a, State b)
        {
            var aParents = new HashSet<State>();
            for (State state = a; state != null; state = state.Parent)
                aParents.Add(state);

            for (State state = b; state != null; state = state.Parent)
                if (aParents.Contains(state)) return state;

            return null;
        }
    }
}