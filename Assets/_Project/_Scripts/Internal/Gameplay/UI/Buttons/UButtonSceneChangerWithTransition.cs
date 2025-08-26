using Definitions.Scenes.Cards;
using Internal.Core.Scenes;
using UI.Images;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Buttons
{
    public class UButtonSceneChangerWithTransition : MonoBehaviour
    {
        [SerializeField] private Button _buttonToBind;
        [SerializeField] private SceneCard _sceneCard;
        [SerializeField] private TransitionImageMover _transitionImageMover;
        
        [Space] [SerializeField] private TransitionImageMover.MoveToTypes _moveTo = TransitionImageMover.MoveToTypes.OverlayScreen;

        private SceneLoader _sceneLoader;

        [Inject]
        private void Construct(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        private void Awake()
        {
            _buttonToBind.onClick.AddListener(() =>
            {
                _transitionImageMover.MoveTo(_moveTo, OnButtonClick);
            });
        }

        private void OnButtonClick()
        {
            _sceneLoader.LoadScene(_sceneCard);
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