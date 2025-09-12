using System;
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
        [field: SerializeField] public ReactiveList<BulletModificatorInInventory> BulletModificators { get; private set; } 
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

        public void AddBulletModificatorInInventory(BulletModificatorSO modificator)
        {
            foreach (var itemInInventory in BulletModificators.List)
            {
                if (itemInInventory.BulletModificatorSO == modificator)
                {
                    itemInInventory.Amount++;
                    return;
                }
            }
            
            BulletModificators.List.Add(new(modificator));
        }

        public void SubtractBulletModificatorInInventory(BulletModificatorSO modificator)
        {
            foreach (var itemInInventory in BulletModificators.List)
            {
                if (itemInInventory.BulletModificatorSO == modificator)
                {
                    if (itemInInventory.Amount > 0) itemInInventory.Amount--;
                    if (itemInInventory.Amount == 0) BulletModificators.List.Remove(itemInInventory);

                    return;
                }
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