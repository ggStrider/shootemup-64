using System;
using System.Collections.Generic;
using UnityEngine;

namespace Definitions.Scenes.CameraBassShake
{
    public class DelaysWaveWithClass<T> : ScriptableObject
    {
        [field: SerializeField] public List<DelayWithT> DelaysWithT { get; private set; } = new();

        public static implicit operator List<DelayWithT>(DelaysWaveWithClass<T> waves)
        {
            return waves.DelaysWithT;
        }

        public int Count => DelaysWithT.Count;
        public DelayWithT this[int index] => DelaysWithT[index];

        [Serializable]
        public class DelayWithT
        {
            [field: SerializeField] public float Delay { get; private set; }
            [field: SerializeField] public T Any { get; private set; }

            public DelayWithT(float delay, T any)
            {
                Delay = delay;
                Any = any;
            }
        }
    }
}