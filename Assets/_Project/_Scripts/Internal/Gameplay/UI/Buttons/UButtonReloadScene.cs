using Internal.Core.Scenes;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Internal.Gameplay.UI.Buttons
{
    public class UButtonReloadScene : MonoBehaviour
    {
        [SerializeField] private Button _buttonToBind;

        [Space] [SerializeField] private bool _reloadWithTransition = true;

        private SceneLoader _sceneLoader;

        [Inject]
        private void Construct(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        private void Awake()
        {
            if (_reloadWithTransition)
            {
                _buttonToBind.onClick.AddListener(_sceneLoader.ReloadSceneWithTransition);
            }
            else
            {
                _buttonToBind.onClick.AddListener(_sceneLoader.ReloadScene);
            }
        }
        
#if UNITY_EDITOR
        private void Reset()
        {
            if (_buttonToBind == null)
            {
                if (TryGetComponent<Button>(out var button)) _buttonToBind = button;
            }
        }
#endif
    }
}