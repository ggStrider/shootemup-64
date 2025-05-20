using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [field: SerializeField] public float CurrentTime { get; private set; }
    [SerializeField] private float _timeToCompleteLevel = 64f;

    [Space] [SerializeField] private bool _startTimerOnStart = true;
    
    public static GameTimer Instance { get; private set; }
    
    public event Action<float> OnCurrentTimeChanged;
    private const float UPDATE_INTERVAL = 0.1f;

    private void Start()
    {
        if (_startTimerOnStart)
        {
            OnCurrentTimeChanged?.Invoke(CurrentTime);
            StartTimer().Forget();
        }

        if (Instance == null)
        {
            Instance = this;
        }
    }
    
    private async UniTaskVoid StartTimer()
    {
        while (CurrentTime < _timeToCompleteLevel)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(UPDATE_INTERVAL), DelayType.Realtime);
            CurrentTime += UPDATE_INTERVAL;
            
            OnCurrentTimeChanged?.Invoke(CurrentTime);
        }
        
        Debug.Log("You've done it!");
    }

    public void SubtractCurrentTime(float delta)
    {
        CurrentTime = Mathf.Max(0, CurrentTime - delta);
    }
}