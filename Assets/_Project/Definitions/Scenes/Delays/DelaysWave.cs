using System.Collections.Generic;
using Definitions.Scenes.Delays.BackgroundChanger;
using NaughtyAttributes;
using UnityEngine;

namespace Definitions.Scenes.CameraBassShake
{
    [CreateAssetMenu(fileName = "New Delays Wave", menuName =
        StaticKeys.PROJECT_NAME + "/Definitions/Delays/Any Delays Waves")]
    public class DelaysWave : ScriptableObject
    {
        [field: SerializeField] public List<float> Delays { get; private set; } = new();

        public static implicit operator List<float>(DelaysWave waves)
        {
            return waves.Delays;
        }

        public int Count => Delays.Count;
        public float this[int index] => Delays[index];

#if UNITY_EDITOR
        [SerializeField] private BackgroundChangeDelays _delays;
        
        [Button]
        public void CopyFromBgChanger()
        {
            Delays = new();
            foreach (var t in _delays.DelaysWith2T)
            {
                var delay = t.Delay;
                Delays.Add(delay);
            }
        }
#endif
    }
}