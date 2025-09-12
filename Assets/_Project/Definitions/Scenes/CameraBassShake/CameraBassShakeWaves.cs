using System.Collections.Generic;
using UnityEngine;

namespace Definitions.Scenes.CameraBassShake
{
    [CreateAssetMenu(fileName = "New Camera Shake Waves", menuName = 
        StaticKeys.PROJECT_NAME + "/Definitions/Camera Shake Waves")]
    public class CameraBassShakeWaves : ScriptableObject
    {
        [field:SerializeField] public List<float> Delays { get; private set; } = new();

        public static implicit operator List<float>(CameraBassShakeWaves waves)
        {
            return waves.Delays;
        }

        public int Count => Delays.Count;
        public float this[int index] => Delays[index];
    }
}