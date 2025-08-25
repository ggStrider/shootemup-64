using UI.Images;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Buttons
{
    public class UButtonSceneChangerWithTransition : MonoBehaviour
    {
        [SerializeField] private Button _buttonToBind;
        [SerializeField] private string _sceneToLoadName;
        [SerializeField] private TransitionImageMover _transitionImageMover;
        
        [Space] [SerializeField] private TransitionImageMover.MoveToTypes _moveTo = TransitionImageMover.MoveToTypes.OverlayScreen;

        private void Awake()
        {
            _buttonToBind.onClick.AddListener(() =>
            {
                _transitionImageMover.OnOverlayed += ChangeScene;
                _transitionImageMover.MoveTo(_moveTo);
            });
        }

        private void ChangeScene()
        {
            SceneManager.LoadScene(_sceneToLoadName);
        }

        private void OnDestroy()
        {
            if (_transitionImageMover != null)
            {
                _transitionImageMover.OnOverlayed -= ChangeScene;
            }
        }
        
#if UNITY_EDITOR
        private void Reset()
        {
            _buttonToBind ??= GetComponent<Button>();
        }
#endif
    }
}