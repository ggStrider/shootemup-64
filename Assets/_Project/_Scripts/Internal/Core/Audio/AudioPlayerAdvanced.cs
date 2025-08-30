using System;
using Internal.Core.Extensions;
using UnityEngine;

namespace Audio
{
    [Serializable]
    public class AudioPlayerAdvanced
    {
        public AudioSource audioSource;
        public AudioClip[] audioClips;

        [Space]
        [Range(0f, 1f)] public float volume = 1f;

        [Space]
        [Range(-3, 3)] public float minPitch = 0.95f;
        [Range(-3, 3)] public float maxPitch = 1.05f;

        [Space] public float defaultPitch = 1;

        public void PlayShotOfRandomSound(bool useRandomPitch = true)
        {
            if (audioClips.Length == 0)
            {
                Debug.LogError($"[{GetType().Name}] No audio clips found");
                return;
            }

            audioSource.pitch = useRandomPitch ? GetRandomPitch() : defaultPitch;
            audioSource.PlayOneShot(GetRandomClip(), volume);
        }

        public void PlayShotOfSound(int audioClipIndex, bool useRandomPitch = true)
        {
            if (audioClips.Length <= audioClipIndex)
            {
                Debug.LogError($"[{GetType().Name}] No audio clips found");
                return;
            }
            audioSource.pitch = useRandomPitch ? GetRandomPitch() : defaultPitch;
            audioSource.PlayOneShot(audioClips[audioClipIndex], volume);
        }

        public void PlayRandomSound(bool useRandomPitch = true)
        {
            if (audioClips.Length == 0)
            {
                Debug.LogError($"[{GetType().Name}] No audio clips found");
                return;
            }

            audioSource.pitch = useRandomPitch ? GetRandomPitch() : defaultPitch;
            
            audioSource.clip = GetRandomClip();
            Debug.Log(audioSource.clip);
            audioSource.Play();
        }

        public float GetRandomPitch()
        {
            return UnityEngine.Random.Range(minPitch, maxPitch);
        }

        public AudioClip GetRandomClip()
        {
            return audioClips.GetRandomElement();
        }
    }
}