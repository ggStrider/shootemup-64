using UnityEngine;

namespace Audio
{
    public class AudioEffectsManager : MonoBehaviour
    {
        [SerializeField] private AudioSource _effectsSource;

        [Space] [SerializeField] private AudioPlayerAdvanced _onEnemyHitEffect;

        public void PlayEnemyHitSound()
        {
            _onEnemyHitEffect.PlayShotOfRandomSound();
        }
    }
}