using System;
using UnityEngine;

namespace Internal.Gameplay.EntitiesShared
{
    public class HealthSystem : MonoBehaviour
    {
        [SerializeField, Min(0)] private int _maxHealth = 3;
        [field: SerializeField] public int CurrentHealth { get; private set; }

        /// <summary>
        /// Invokes on damage taken, passing current health
        /// </summary>
        public event Action<int> OnDamageTaken;

        /// <summary>
        /// Invokes on healed, passing current health
        /// </summary>
        public event Action<int> OnHealed;

        /// <summary>
        /// Invokes when current health is zero or less than zero
        /// </summary>
        public event Action OnDeath;

        private void Awake()
        {
            InitializeHealth();
        }

        public void InitializeHealth()
        {
            CurrentHealth = _maxHealth;
        }
        
        private void OnDestroy()
        {
            OnDamageTaken = null;
            OnHealed = null;
            OnDeath = null;
        }

        public void TakeDamage(int amount)
        {
            // Debug.Log($"[{GetType().Name}] Damage {amount}");
            if (CurrentHealth == 0) return;

            if (amount <= 0)
            {
                Debug.LogError($"[{GetType().Name}] Damage amount cannot be <= 0");
                return;
            }

            CurrentHealth = Mathf.Max(0, CurrentHealth - amount);

            OnDamageTaken?.Invoke(CurrentHealth);
            if (CurrentHealth <= 0)
            {
                OnDeath?.Invoke();
            }
        }

        public void ForceDeath()
        {
            TakeDamage(CurrentHealth);
        }

        public void Heal(int amount)
        {
            CurrentHealth = Mathf.Min(CurrentHealth + amount, _maxHealth);
            OnHealed?.Invoke(amount);
        }
    }
}