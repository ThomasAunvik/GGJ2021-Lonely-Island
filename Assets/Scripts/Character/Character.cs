using LonelyIsland.System;
using UnityEngine;

namespace LonelyIsland.Character
{
    public interface ICharacter
    {
        public float Health { get; }
        public float Damage { get; }
        public float TotalMaxHealth { get; }
        public float TotalMaxDamage { get; }
    }

    public class Character : MonoBehaviour, ICharacter
    {
        [Header("Health")]
        [SerializeField] protected float HealthMax = 100;
        [SerializeField] protected float HealthMin = 0;
        [SerializeField] protected float HealthMultiplier = 1;

        [Header("Damage")]
        [SerializeField] protected float DamageMax = 10;
        [SerializeField] protected float DamageMin = 1;
        [SerializeField] protected float DamageMultiplier = 1;

        protected float health;
        protected virtual float SetHealth(float newHealth) { return health = Mathf.Clamp(newHealth, HealthMin, TotalMaxHealth); }
        public virtual float Health { get { return health; } }
        public virtual float Damage { get { return DamageMultiplier; } }

        public virtual float TotalMaxHealth { get { return HealthMax; } }
        public virtual float TotalMaxDamage { get { return HealthMin; } }

        private void Start()
        {
            SetHealth(TotalMaxHealth);
        }
    }
}
