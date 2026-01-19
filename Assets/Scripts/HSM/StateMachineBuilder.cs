using System.Collections.Generic;
using System.Reflection;

namespace HSM
{
    public class StateMachineBuilder
    {
        private readonly State _root;

        public StateMachineBuilder(State root)
            => _root = root;

        public StateMachine Build()
        {
            var machine = new StateMachine(_root);
            Wire(_root, machine, new HashSet<State>());
            return machine;
        }
        
        private void Wire(State state, StateMachine machine, HashSet<State> visited)
        {
            // ts is reflection hell nocap
            
            if (state == null) return;
            if (!visited.Add(state)) return; // State is already wired

            var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic |
                        BindingFlags.FlattenHierarchy;
            var machineField = typeof(State).GetField("Machine", flags);
            machineField?.SetValue(state, machine);

            foreach (FieldInfo field in state.GetType().GetFields(flags))
            {
                if (!typeof(State).IsAssignableFrom(field.FieldType)) continue; // Only consider fields that are State
                if (field.Name == "Parent") continue;                           // Skip back-edge to parent

                var child = (State)field.GetValue(state);
                if (child == null) continue;
                if (!ReferenceEquals(child.Parent, state)) continue;            // Ensure it's actually our direct child

                Wire(child, machine, visited);                                  // Recurse into the child
            }
        }
    }
}