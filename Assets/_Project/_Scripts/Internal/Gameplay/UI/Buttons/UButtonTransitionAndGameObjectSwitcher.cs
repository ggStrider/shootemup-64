using UI.Images;
using UnityEngine;
using UnityEngine.UI;

namespace Internal.Gameplay.UI.Buttons
{
    public class UButtonTransitionAndGameObjectSwitcher : MonoBehaviour
    {
        [SerializeField] private Button _buttonToBind;
        [SerializeField] private TransitionImageMover.MoveToTypes _moveToWhenSwitched 
            = TransitionImageMover.MoveToTypes.ToLeft;
        
        [Space]
        [SerializeField] private GameObject _toEnable;
        [SerializeField] private GameObject _toDisable;

        private void Awake()
        {
            _buttonToBind.onClick.AddListener(OverlayAndSwitch);
        }

        private void OverlayAndSwitch()
        {
            TransitionImageMover.Instance.MoveTo(TransitionImageMover.MoveToTypes.OverlayScreen,
                SwitchGameObjects);
        }

        private void SwitchGameObjects()
        {
            _toDisable.SetActive(false);
            _toEnable.SetActive(true);
            
            TransitionImageMover.Instance.MoveTo(_moveToWhenSwitched);
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