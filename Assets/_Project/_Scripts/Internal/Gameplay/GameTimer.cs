using System;
using Cysharp.Threading.Tasks;
using Internal.Core.Reactive;
using Internal.Core.Signals;
using UnityEngine;
using Zenject;

namespace Internal.Gameplay
{
    public class GameTimer : MonoBehaviour
    {
        [SerializeField] private float _timeToCompleteLevel = 64f;

        [Space]
        [SerializeField] private bool _startTimerOnStart = true;

        [Space] [SerializeField] private float _subtractTimeWhenHitFakeEnemy = 1.5f;

        public static GameTimer Instance { get; private set; }
        public ReactiveVariable<float> CurrentTimeReactive { get; private set; }

        private SignalBus _signalBus;

        private const float UPDATE_TIME_INTERVAL = 0.1f;

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<EnemyDieSignal>(() => SubtractCurrentTime(_subtractTimeWhenHitFakeEnemy));
        }

        private void OnDisable()
        {
            _signalBus.TryUnsubscribe<EnemyDieSignal>(() => SubtractCurrentTime(_subtractTimeWhenHitFakeEnemy));
        }

        private void Awake()
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
