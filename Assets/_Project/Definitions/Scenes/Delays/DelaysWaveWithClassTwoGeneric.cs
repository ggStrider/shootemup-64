using System;
using System.Collections.Generic;
using UnityEngine;

namespace Definitions.Scenes.CameraBassShake
{
    public class DelaysWaveWithClassTwoGeneric<T1, T2> : ScriptableObject
    {
        [field: SerializeField] public List<DelayWith2T> DelaysWith2T { get; private set; } = new();

        public static implicit operator List<DelayWith2T>(DelaysWaveWithClassTwoGeneric<T1, T2> waves)
        {
            return waves.DelaysWith2T;
        }

        public int Count => DelaysWith2T.Count;
        public DelayWith2T this[int index] => DelaysWith2T[index];

        [Serializable]
        public class DelayWith2T
        {
            [field: SerializeField] public float Delay { get; private set; }
            [field: SerializeField] public T1 Any1 { get; private set; }
            [field: SerializeField] public T2 Any2 { get; private set; }

            public DelayWith2T(float delay, T1 any1 = default, T2 any2 = default)
            {
                Delay = delay;
                Any1 = any1;
                Any2 = any2;
            }

            public void SetDelay(float delay)
            {
                Delay = delay;
            }
        }
    }
}