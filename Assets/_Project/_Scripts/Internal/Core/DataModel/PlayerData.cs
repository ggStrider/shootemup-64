using System;
using System.Linq;
using Definitions.BulletModificators.Scripts;
using Internal.Core.Reactive;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Internal.Core.DataModel
{
    [Serializable]
    public class PlayerData : IInitializable, IDisposable
    {
        [field: SerializeField] public ReactiveVariable<int> Coins { get; private set; } = new();

        [field: Space]
        [field: SerializeField]
        public ReactiveList<BulletModificatorInInventory> BulletModificators { get; private set; }
            = new();

        public void AddCoins(int addValue)
        {
            if (addValue <= 0)
            {
                Debug.LogError($"[{GetType().Name}] Coins 'add value' cannot add <= 0");
                return;
            }

            Coins.Value += addValue;
        }

        public void SubtractCoins(int subtractValue)
        {
            if (subtractValue <= 0)
            {
                Debug.LogError($"[{GetType().Name}] Coins 'subtract value' cannot add <= 0");
                return;
            }

            // Clamps, so value never < 0
            Coins.Value = Mathf.Max(Coins.Value - subtractValue, 0);
        }

        public BulletModificatorInInventory TryGetBulletModificatorInInventoryBy(
            BulletModificatorSO bulletModificatorSO)
        {
            return BulletModificators.List.FirstOrDefault(item =>
                item.BulletModificatorSO == bulletModificatorSO);
        }

        public void AddBulletModificatorInInventory(BulletModificatorSO modificator)
        {
            var itemInInventory = TryGetBulletModificatorInInventoryBy(modificator);
            if (itemInInventory != null)
            {
                itemInInventory.Amount++;
            }
            else
            {
                BulletModificators.List.Add(new(modificator));
            }
        }

        public void SubtractBulletModificatorInInventory(BulletModificatorSO modificator)
        {
            var itemInInventory = TryGetBulletModificatorInInventoryBy(modificator);
            if (itemInInventory != null)
            {
                if (itemInInventory.Amount > 0) itemInInventory.Amount--;
                if (itemInInventory.Amount == 0) BulletModificators.List.Remove(itemInInventory);
            }
        }

        public void Initialize()
        {
            SceneManager.sceneLoaded += OnSceneChanged;
        }

        public void Dispose()
        {
            SceneManager.sceneLoaded -= OnSceneChanged;
            SaveSystem.Save(this);
        }

        private void OnSceneChanged(Scene scene, LoadSceneMode loadSceneMode)
        {
            SaveSystem.Save(this);
        }
    }
}