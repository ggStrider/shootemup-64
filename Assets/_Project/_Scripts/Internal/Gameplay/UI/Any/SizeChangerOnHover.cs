using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Any
{
    public class SizeChangerOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private RectTransform _target;
        
        [Space]
        [SerializeField] private Vector2 _sizeOnHover;
        [SerializeField] private Vector2 _sizeOnUnHover;
        [SerializeField, Min(0)] private float _durationPerAnimation = 0.2f;

        private Tween _currentTween;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            StartAnimation(_sizeOnHover);
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            StartAnimation(_sizeOnUnHover);
        }

        private void StartAnimation(Vector2 newSize)
        {
            _currentTween?.Kill();
            
            // for right visualisation
            var newSizeWithZ1 = new Vector3(newSize.x, newSize.y, 1);
            _currentTween = _target.DOScale(newSizeWithZ1, _durationPerAnimation);
        }
        
        #if UNITY_EDITOR
        private void Reset()
        {
            if (_target == null)
            {
                _target = GetComponent<RectTransform>();
            }
            if (_sizeOnHover == Vector2.zero)
            {
                _sizeOnHover = _target.localScale + new Vector3(0.1f, 0.1f);
            }

            if (_sizeOnUnHover == Vector2.zero)
            {
                _sizeOnUnHover = _target.localScale;
            }
        }
        #endif
    }
}
