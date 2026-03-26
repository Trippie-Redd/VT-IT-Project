using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HSM
{
    public abstract class State
    {
        public readonly StateMachine Machine;
        public readonly State Parent;
        public State ActiveChild;

        private readonly List<IActivity> _activities = new();
        public IReadOnlyList<IActivity> Activities => _activities;

        protected State(StateMachine machine, State parent = null)
            => (Machine, Parent) = (machine, parent);

        public void Add(IActivity activity)
        {
            if (activity != null) 
                _activities.Add(activity);
        }
    
        /// <summary>
        /// Initial child to enter (ayo pause) when this state starts (null = this is the leaf)
        /// </summary>
        protected virtual State GetInitialState() => null;
        
        /// <summary>
        /// Target state to switch to this frame (null = stay in current state)
        /// </summary>
        protected virtual State GetTransition() => null;

        /// <summary>
        /// Returns whether you can currently transition to this state
        /// </summary>
        public virtual bool CanTransitionTo() => true;
        
        // Lifecycle hooks
        protected virtual void OnEnter()                 {}
        protected virtual void OnExit()                  {}
        protected virtual void OnUpdate(float deltaTime) {}

        // TODO - we gon check up on this
        internal void Enter()
        {
            if (Parent != null) Parent.ActiveChild = this;
            OnEnter();
            State init = GetInitialState();
            init?.Enter();
        }
        internal void Exit()
        {
            ActiveChild?.Exit();
            ActiveChild = null;
            OnExit();
        }
        internal void Update(float deltaTime)
        {
            State newState = GetTransition();
            if (newState != ActiveChild)
            {
                Machine.Sequencer.RequestTransition(ActiveChild, newState);
                
                Debug.Assert(ActiveChild != null);
                Debug.Assert(newState != null);
                Debug.Log($"Transition requested from {ActiveChild.GetType()} to {newState.GetType()}");
                
                return;
            }
            
            ActiveChild?.Update(deltaTime);
            OnUpdate(deltaTime);

            if (ActiveChild == null)
            {
                //_LogCurrentTree();
            }
        }
        
        /// <summary>
        /// Returns the deepest currently active descendant state
        /// </summary>
        public State Leaf()
        {
            State s = this;
            while (s.ActiveChild != null)
                s = s.ActiveChild;
            
            return s;
        }

        /// <summary>
        /// Yields this state and then each ancestor up to the root (self -> parent -> ... -> root)
        /// </summary>
        public IEnumerable<State> PathToRoot()
        {
            for (State s = this; s != null; s = s.Parent)
                yield return s;
        }

        public void LogCurrentTree()
        {
            string currentTree = "";
            var path = PathToRoot();
            foreach (var state in PathToRoot().AsEnumerable().Reverse())
            {
                currentTree += state + "->";
            }

            currentTree = currentTree[..^2];

            Debug.Log($"{currentTree}");
        }
    }
}