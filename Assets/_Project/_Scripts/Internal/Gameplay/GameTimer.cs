using System;
using Cysharp.Threading.Tasks;
using Internal.Core.Reactive;
using UnityEngine;

namespace Internal.Gameplay
{
    public class GameTimer : MonoBehaviour
    {
        [SerializeField] private float _timeToCompleteLevel = 64f;

        [Space]
        [SerializeField] private bool _startTimerOnStart = true;

        public static GameTimer Instance { get; private set; }

        public ReactiveVariable<float> CurrentTimeReactive;

        private const float UPDATE_TIME_INTERVAL = 0.1f;

        private void Start()
        {
            if (_startTimerOnStart)
            {
                InitializeTimer();
                CountTime().Forget();
            }

            if (Instance == null)
            {
                Instance = this;
            }
        }

        public void InitializeTimer(float timerStartValue = 0)
        {
            CurrentTimeReactive = new(timerStartValue);
        }

        private async UniTaskVoid CountTime()
        {
            while (CurrentTimeReactive < _timeToCompleteLevel)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(UPDATE_TIME_INTERVAL), DelayType.Realtime);
                CurrentTimeReactive.Value += UPDATE_TIME_INTERVAL;
            }

            Debug.Log("You've done it!");
        }

        public void SubtractCurrentTime(float delta)
        {
            CurrentTimeReactive.Value = Mathf.Max(0, CurrentTimeReactive.Value - delta);
        }
    }
}
