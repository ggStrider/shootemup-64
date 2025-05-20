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
            if (IsSessionsExist())
            {
                DestroyImmediate(gameObject);
            }
            else
            {
                DontDestroyOnLoad(this);
                SetupManager();
            }
        }

        private bool IsSessionsExist()
        {
            return FindObjectsByType<MusicManager>(FindObjectsSortMode.None).Length > 1;
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