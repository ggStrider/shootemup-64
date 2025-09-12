using System;
using Shoot;
using UnityEngine;

namespace Definitions.BulletModificators.Scripts
{
    public abstract class BulletModificatorSO : ScriptableObject
    {
        [field: SerializeField] public BulletModificatorUI BulletModificatorUI;
        [field: SerializeField, Min(0)] public int Price { get; private set; } = 10;
        
        public abstract void ApplyModificator(BulletBehaviour bullet);
    }
    
    
    [Serializable]
    public class BulletModificatorUI
    { 
        [field: SerializeField] public Sprite ModificatorIcon { get; private set; }
        [field: SerializeField] public Color IconColor { get; private set; } = Color.white;
    }
}
