using System;
using UnityEngine;

namespace ImprovedTimers
{
    public abstract class Timer : IDisposable
    {
        public float CurrentTime { get; protected set; }
        public bool IsRunning { get; private set; }

        protected float _initalTime;

        public float Progress => Mathf.Clamp(CurrentTime / _initalTime, 0, 1);

        public Action OnTimerStart = delegate { };
        public Action OnTimerStop  = delegate { };
        
        private bool _disposed;

        protected Timer(float value)
        {
            _initalTime = value;
        }

        public void Start()
        {
            CurrentTime = _initalTime;
            if (IsRunning) return;
            
            IsRunning = true;
            TimerManager.RegisterTimer(this);
            OnTimerStart.Invoke();
        }

        public void Stop()
        {
            if (!IsRunning) return;

            IsRunning = false;
            TimerManager.DeregisterTimer(this);
            OnTimerStop.Invoke();
        }
        
        public abstract void Tick();
        public abstract bool IsFinished { get; }

        public void Pause()  => IsRunning = false;
        public void Resume() => IsRunning = true;

        public virtual void Reset() => CurrentTime = _initalTime;

        public virtual void Reset(float newTime)
        {
            _initalTime = newTime;
            Reset();
        }
        
        ~Timer()
        {
            Dispose(false);
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                TimerManager.DeregisterTimer(this);
            }

            _disposed = true;
        }
    }
}