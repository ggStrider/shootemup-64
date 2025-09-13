using Definitions.Scenes.Cards;
using Internal.Core.Scenes;
using NaughtyAttributes;
using TMPro;
using UI.Images;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Buttons
{
    public class UButtonSceneChangerWithTransition : MonoBehaviour
    {
        [SerializeField] private Button _buttonToBind;

        [Space] [SerializeField] private bool _writeAutoSceneName = true;
        [SerializeField, ShowIf(nameof(_writeAutoSceneName))] private TextMeshProUGUI _levelNamePlaceholder;
        
        [Space]
        [SerializeField] private SceneCard _sceneCard;
        [SerializeField] private bool _initializeOnAwake = true;
        [SerializeField] private bool _playMusicOfNextSceneInstantly = false;

        private SceneLoader _sceneLoader;

        [Inject]
        private void Construct(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        private void Awake()
        {
            if (_initializeOnAwake)
            {
                Initialize();
            }
        }

        public void SetSceneCardAndInitialize(SceneCard sceneCard)
        {
            _sceneCard = sceneCard;
            Initialize();
        }

        private void Initialize()
        {
            if (_writeAutoSceneName)
            {
                if (_levelNamePlaceholder == null || _sceneCard == null)
                {
                    Debug.LogError($"[{nameof(UButtonSceneChangerWithTransition)}] Missing references! " +
                                   $"LevelNamePlaceholder={_levelNamePlaceholder}, SceneCard={_sceneCard}");
                    return;
                }

                _levelNamePlaceholder.text = _sceneCard.ScenePreviewName;
            }

            if (_buttonToBind == null)
            {
                Debug.LogError($"[{nameof(UButtonSceneChangerWithTransition)}] ButtonToBind is null!");
                return;
            }

            _buttonToBind.onClick.AddListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            _sceneLoader.LoadSceneWithTransition(_sceneCard, _playMusicOfNextSceneInstantly);
        }
        
#if UNITY_EDITOR
        private void Reset()
        {
            if (!_buttonToBind)
            {
                _buttonToBind = GetComponent<Button>();
            }
        }
#endif
    }
}