using Shoot;
using UnityEngine;

namespace Definitions.BulletModificators.Scripts
{
    public abstract class BulletModificatorSO : ScriptableObject
    {
        public abstract void ApplyModificator(BulletBehaviour bullet);
    }
}