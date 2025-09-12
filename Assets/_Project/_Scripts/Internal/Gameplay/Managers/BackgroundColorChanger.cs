using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Definitions.Scenes.Cards;
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
        [SerializeField]private SpriteRenderer _backgroundRenderer;

        private CancellationTokenSource _changingBackgroundCts;

        private SceneCard _sceneCard;
        private SignalBus _signalBus;

        [Inject]
        private void Construct(SignalBus signalBus, SceneCardHolder sceneCardHolder)
        {
            _signalBus = signalBus;
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

        private async UniTask RandomizeBackgroundColorAsync(int waveIndex, CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    var randomColor = Random.ColorHSV(
                        0f, 1f,
                        0.5f, 1f,
                        0.5f, 1f
                    );
                    randomColor.a = 1;
                    _backgroundRenderer.color = randomColor;
                    
                    _signalBus.Fire(new BackgroundChangedSignal(
                        _backgroundRenderer, _backgroundRenderer.color));
                    
                    await UniTask.WaitForSeconds(
                        _sceneCard.LevelWaves[waveIndex].DelayToChangeBackground, cancellationToken: token);
                }
            }
            catch (OperationCanceledException)
            {
            }
        }
    }
}