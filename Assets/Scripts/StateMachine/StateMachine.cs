using System.Collections.Generic;

namespace StateMachine
{
    public class StateMachine
    {
        public readonly State Root;
        public readonly TransitionSequencer Sequencer;
        private bool _started;

        public StateMachine(State root)
            => (Root, Sequencer) = (root, new TransitionSequencer(this));

        public void Start()
        {
            if (_started) return;

            _started = true;
            Root.Enter();
        }

        public void Tick(float deltaTime)
        {
            if (!_started) Start();
            Sequencer.Tick(deltaTime);
        }

        internal void InternalTick(float deltaTime)
            => Root.Update(deltaTime);

        /// <summary>
        /// Perform the actual switch from 'from' to 'to' by exiting up to the shared ancestor, then entering down to the target
        /// </summary>
        public void ChangeState(State from, State to)
        {
            if (from == to || from == null || to == null) return;

            State lca = TransitionSequencer.Lca(from, to);

            for (State state = from; state != lca; state = state.Parent) 
                state.Exit();

            var stack = new Stack<State>();
            for (State state = to; state != lca; state = state.Parent)
                stack.Push(state);
            
            while (stack.Count > 0) 
                stack.Pop().Enter();
        }
    }   
}