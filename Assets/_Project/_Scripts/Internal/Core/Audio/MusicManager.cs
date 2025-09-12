using System;
using DG.Tweening;
using UnityEngine;

namespace Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicManager : MonoBehaviour
    {
        [SerializeField] private float _volumeOnSetup = 0.6f;
        [SerializeField] private AudioPlayerAdvanced _audioPlayerAdvanced = new();

        private void Awake()
        {
            SetupManager();
        }

        private void SetupManager()
        {
            if (!_audioPlayerAdvanced.audioSource.isPlaying)
            {
                _audioPlayerAdvanced.PlayRandomSound(useRandomPitch: false);
            }

            _audioPlayerAdvanced.audioSource.volume = _volumeOnSetup;
            Invoke(nameof(SetupManager), _audioPlayerAdvanced.audioSource.clip.length - _audioPlayerAdvanced.audioSource.time);
        }

        private void OnDestroy()
        {
            CancelInvoke(nameof(SetupManager));
        }

        public void StopPlayingMusic()
        {
            _audioPlayerAdvanced.audioSource.Stop();
        }

        public void FadeCurrentClip(float timeToFade = 0.4f, Action onFaded = null)
        {
            CancelInvoke(nameof(SetupManager));
            
            DOVirtual.Float(_audioPlayerAdvanced.audioSource.volume, 0, timeToFade,
                x => _audioPlayerAdvanced.audioSource.volume = x)
                .OnComplete(() => onFaded?.Invoke());
        }

        public void StartAndUnFadeClip(AudioClip clip, float unFadeDuration = 0.7f)
        {
            CancelInvoke(nameof(SetupManager));
            
            _audioPlayerAdvanced.audioSource.volume = 0;
            _audioPlayerAdvanced.audioSource.clip = clip;
            
            _audioPlayerAdvanced.audioSource.Play();
            
            DOVirtual.Float(_audioPlayerAdvanced.audioSource.volume, _volumeOnSetup, unFadeDuration,
                x => _audioPlayerAdvanced.audioSource.volume = x);
        }

#if UNITY_EDITOR
        private void Reset()
        {
            UnityEditor.EditorApplication.delayCall += () =>
            {
                _audioPlayerAdvanced.audioSource ??= GetComponent<AudioSource>();
            };
        }
#endif
    }
}