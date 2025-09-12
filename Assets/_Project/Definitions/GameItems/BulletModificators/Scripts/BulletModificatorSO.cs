using System;
using Shoot;
using UnityEngine;

namespace Definitions.BulletModificators.Scripts
{
    public abstract class BulletModificatorSO : GameItem
    {
        [field: SerializeField, Min(0)] public int Price { get; private set; } = 10;
        
        public abstract void ApplyModificator(BulletBehaviour bullet);
    }
}
