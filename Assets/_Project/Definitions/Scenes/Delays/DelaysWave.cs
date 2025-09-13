using System.Collections.Generic;
using UnityEngine;

namespace Definitions.Scenes.CameraBassShake
{
    [CreateAssetMenu(fileName = "New Delays Wave", menuName = 
        StaticKeys.PROJECT_NAME + "/Definitions/Delays/Any Delays Waves")]
    public class DelaysWave : ScriptableObject
    {
        [field:SerializeField] public List<float> Delays { get; private set; } = new();

        public static implicit operator List<float>(DelaysWave waves)
        {
            return waves.Delays;
        }

        public int Count => Delays.Count;
        public float this[int index] => Delays[index];
    }
}