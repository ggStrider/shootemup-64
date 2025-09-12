using DG.Tweening;
using UnityEngine;

namespace Internal.Gameplay.UI.Any
{
    public class ScaleAnimationOnEnable : MonoBehaviour
    {
        [SerializeField] private Vector3 _setOnEnable = Vector3.one;
        [SerializeField] private Vector3 _setOnDisable = Vector3.zero;

        [Space] [SerializeField] private float _tweenDuration = 0.6f;

        private void OnEnable()
        {
            transform.localScale = _setOnDisable;
            
            transform.DOKill();
            transform.DOScale(_setOnEnable, _tweenDuration);
        }

        private void OnDisable()
        {
            transform.localScale = _setOnDisable;
        }
    }
}
