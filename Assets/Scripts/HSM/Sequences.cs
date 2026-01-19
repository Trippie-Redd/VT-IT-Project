using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HSM
{
    public interface ISequence
    {
        bool IsDone { get; }
        void Start();
        bool Update();
    }

    public delegate Task PhaseStep(CancellationToken ct);

    public class ParallelPhase : ISequence
    {
        private readonly List<PhaseStep> _steps;
        private readonly CancellationToken _ct;
        private List<Task> _tasks;

        public bool IsDone { get; private set; }
        
        public ParallelPhase(List<PhaseStep> steps, CancellationToken ct)
            => (_steps, _ct) = (steps, ct);

        public void Start()
        {
            if (_steps == null || _steps.Count == 0)
            {
                IsDone = true;
                return;
            }

            _tasks = new List<Task>(_steps.Count);

            foreach (var step in _steps)
                _tasks.Add(step(_ct));
        }

        public bool Update()
        {
            if (IsDone) return true;
            IsDone = (_tasks == null || _tasks.TrueForAll(t => t.IsCompleted));
            return IsDone;
        }
    }
    
    public class SequentialPhase : ISequence
    {
        private readonly List<PhaseStep> _steps;
        private readonly CancellationToken _ct;
        private int _index = -1;
        private Task _current;
        
        public bool IsDone { get; private set; }

        public SequentialPhase(List<PhaseStep> steps, CancellationToken ct)
            => (_steps, _ct) = (steps, ct);

        public void Start()
            => _Next();

        public bool Update()
        {
            if (IsDone) return true;
            if (_current == null || _current.IsCompleted) _Next();
            return IsDone;
        }

        private void _Next()
        {
            _index++;

            if (_index >= _steps.Count)
            {
                IsDone = true;
                return;
            }

            _current = _steps[_index](_ct);
        }
    }
    
    public class NoopPhase : ISequence
    {
        public bool IsDone { get; private set; }
        public void Start()  => IsDone = true;
        public bool Update() => IsDone;
    }
}