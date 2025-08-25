using Audio;
using UnityEngine;

namespace Enemy.OnEnemyDestroyFuncs
{
    public class PlaySoundOnEnemyDestroy : MonoBehaviour, IOnEnemyDestroy
    {
        [SerializeField] private AudioPlayerAdvanced _audioPlayer;
        
        public void OnEnemyDestroy()
        {
            var go = new GameObject
            {
                name = "SoundOnEnemyDie"
            };
            
            var aSource = go.AddComponent<AudioSource>();
            aSource.clip = _audioPlayer.GetRandomClip();
            aSource.pitch = _audioPlayer.GetRandomPitch();
            aSource.Play();
            
            Destroy(go, _audioPlayer.audioClips[0].length);
        }
    }
}