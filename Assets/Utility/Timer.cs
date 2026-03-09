using UnityEngine;
using UnityEngine.Events;

namespace Utility
{
    [System.Serializable]
    public abstract class Timer
    {
        public float duration;
        public UnityEvent OnFinished = new();
        public bool Finished => finished;
        public float TimeRemaining => timeRemaining;

        bool finished = false;

        float timeRemaining;

        public Timer(float duration)
        {
            this.duration = duration;
        }

        public abstract void Update();

        protected void UpdateBy(float deltaTime)
        {
            if (!finished)
            {
                if (timeRemaining >= 0)
                {
                    timeRemaining -= deltaTime;
                }
                else
                {
                    finished = true;
                    OnFinished.Invoke();
                }
            }
        }

        public void Reset()
        {
            timeRemaining = duration;
            finished = false;
        }
    }

    [System.Serializable]
    public class UnscaledTimer : Timer
    {
        public UnscaledTimer(float duration) : base(duration)
        { }

        public override void Update()
        {
            UpdateBy(Time.unscaledDeltaTime);
        }
    }

    [System.Serializable]
    public class FixedTimer : Timer
    {
        public FixedTimer(float duration) : base(duration)
        { }

        public override void Update()
        {
            UpdateBy(Time.fixedDeltaTime);
        }
    }

    [System.Serializable]
    public class NormalTimer : Timer
    {
        public NormalTimer (float duration) : base(duration)
        { }
        public override void Update()
        {
            UpdateBy(Time.deltaTime);
        }
    }
}