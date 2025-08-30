using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Internal.Core.DataModel
{
    [Serializable]
    public class PlayerData : IInitializable, IDisposable
    {
        [SerializeField] private int _coins;
        public int Coins => _coins;
        
        private void TrySetCoins(int value)
        {
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
