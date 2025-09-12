using System;
using Definitions.BulletModificators.Scripts;
using UnityEngine;

namespace Internal.Core.DataModel
{
    [Serializable]
    public class BulletModificatorInInventory
    {
        [field: SerializeField] public BulletModificatorSO BulletModificatorSO { get; private set; }
        [field: SerializeField] public int Amount;

        public BulletModificatorInInventory(BulletModificatorSO so, int amount = 1)
        {
            BulletModificatorSO = so;

            if (amount < 0)
            {
                Debug.LogError($"[{nameof(BulletModificatorInInventory)}] Init amount cannot be < 0");
                amount = 1;
            }

            Amount = amount;
        }
    }
}