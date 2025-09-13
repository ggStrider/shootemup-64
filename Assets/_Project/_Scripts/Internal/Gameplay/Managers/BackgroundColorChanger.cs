using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Definitions.Scenes.Cards;
using Definitions.Scenes.Delays.BackgroundChanger;
using Internal.Core.Extensions;
using Internal.Core.Scenes;
using Internal.Core.Signals;
using Tools;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Internal.Gameplay.Managers
{
    public class BackgroundColorChanger : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _backgroundRenderer;

        [Space] [SerializeField] private bool _testChangeSmoothlyAlways;

        private CancellationTokenSource _changingBackgroundCts;

        private BackgroundChangeDelays _backgroundChangeDelaysData;
        private SceneCard _sceneCard;

        private SignalBus _signalBus;

        [Inject]
        private void Construct(SignalBus signalBus, SceneCardHolder sceneCardHolder)
        {
            _signalBus = signalBus;
            _backgroundChangeDelaysData = sceneCardHolder.CurrentSceneCard.BackgroundChangeDelays;
            _sceneCard = sceneCardHolder.CurrentSceneCard;
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<GameEndSignal>(StopChangingColor);
        }

        private void OnDisable()
        {
            _signalBus.TryUnsubscribe<GameEndSignal>(StopChangingColor);
        }

        public void Initialize()
        {
            MyUniTaskExtensions.SafeCancelAndCleanToken(ref _changingBackgroundCts,
                createNewTokenAfter: true);

            RandomizeBackgroundColorAsync(0, _changingBackgroundCts.Token).Forget();
        }

        private void OnDestroy()
        {
            StopChangingColor();
        }

        private void StopChangingColor()
        {
            MyUniTaskExtensions.SafeCancelAndCleanToken(ref _changingBackgroundCts);
        }

        private async UniTask RandomizeBackgroundColorAsync(int startIndex, CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    var (targetColor, delay, smooth) = GetNextColorAndDelay(startIndex);

                    if (smooth)
                        await SmoothChangeColor(targetColor, delay, token);
                    else
                        await InstantChangeColor(targetColor, delay, token);

                    startIndex++;
                }
            }
            catch (OperationCanceledException)
            {
            }
        }

        private (Color targetColor, float delay, bool smooth) GetNextColorAndDelay(int waveIndex)
        {
            if (_sceneCard.UseRandomBackgroundColor)
            {
                Color color = new();
                color = color.GenerateRandomColor();
                
                var delay = _sceneCard.UseLevelWaveAsDelay
                    ? _sceneCard.LevelWaves[waveIndex % _sceneCard.LevelWaves.Length].DelayToChangeBackground
                    : 1f;
                
                return (color, delay, _testChangeSmoothlyAlways);
            }

            if (_sceneCard.UseLevelWaveAsDelay)
            {
                var wave = _sceneCard.LevelWaves[waveIndex % _sceneCard.LevelWaves.Length];
                return (_backgroundRenderer.color, wave.DelayToChangeBackground, _testChangeSmoothlyAlways);
            }

            var delayData = _backgroundChangeDelaysData[waveIndex % _backgroundChangeDelaysData.Count];
            return (delayData.Any1, delayData.Delay, delayData.Any2);
        }

        private async UniTask SmoothChangeColor(Color targetColor, float duration, CancellationToken token)
        {
            var startColor = _backgroundRenderer.color;
            float elapsed = 0f;

            while (elapsed < duration && !token.IsCancellationRequested)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                _backgroundRenderer.color = Color.Lerp(startColor, targetColor, t);
                _signalBus.Fire(new BackgroundChangedSignal(
                    _backgroundRenderer, _backgroundRenderer.color, _backgroundRenderer.color.GetComplementary(),
                        false));
                
                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }
            
            _signalBus.Fire(new BackgroundChangedSignal(
                _backgroundRenderer, _backgroundRenderer.color, _backgroundRenderer.color.GetComplementary(),
                true));
        }

        private async UniTask InstantChangeColor(Color targetColor, float delay, CancellationToken token)
        {
            _backgroundRenderer.color = targetColor;
            _signalBus.Fire(new BackgroundChangedSignal(
                _backgroundRenderer, _backgroundRenderer.color, _backgroundRenderer.color.GetComplementary(),
                    true));
            
            await UniTask.WaitForSeconds(delay, cancellationToken: token);
        }
    }
}