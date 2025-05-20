using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Images
{
    public class Fader : MonoBehaviour
    {
        [SerializeField] private Image _imageToFade;
        [SerializeField] private float _fadeDuration = 0.5f;

        [Space] [SerializeField] private bool _fadeOutOnStart;
        
        public event Action OnFadeIn;
        public event Action OnFadeOut;

        private void Start()
        {
            if (_fadeOutOnStart)
            {
                FadeOut();
            }
        }

        public void FadeIn()
        {
            _imageToFade.DOFade(1.0f, _fadeDuration).OnComplete(() => {
                OnFadeIn?.Invoke();
            });
        }
        
        public void FadeOut()
        {
            _imageToFade.DOFade(0, _fadeDuration).OnComplete(() => {
                OnFadeOut?.Invoke();
            });
        }
    }
}
