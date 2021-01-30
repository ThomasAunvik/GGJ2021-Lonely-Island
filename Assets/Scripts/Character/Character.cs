﻿using LonelyIsland.System;
using UnityEngine;

namespace LonelyIsland.Characters
{
    public interface ICharacter
    {
        public float Health { get; }
        public float Damage { get; }
        public float TotalMaxHealth { get; }
        public float TotalMaxDamage { get; }
        public float TakeDamage(float damage);
        public void Attack(ICharacter[] characters);
    }

    public class Character : MonoBehaviour, ICharacter
    {
        [Header("Movement")]
        [SerializeField] protected float MovementSpeed = 1;
        [SerializeField] protected float MovementSprintSpeed = 2;
        [SerializeField] protected float RotateSpeed = 1;
        [SerializeField] protected float RotateSprintSpeed = 1;
        [SerializeField] protected float JumpForce = 1;
        [SerializeField] protected float Gravity = -1;
        protected Vector3 Velocity;

        [SerializeField] protected bool Sprint = false;
        [SerializeField] protected bool ToggleSpring = false;
        protected bool IsSprinting { get { return Sprint || ToggleSpring; } }

        [Header("Health")]
        [SerializeField] protected float HealthMax = 100;
        [SerializeField] protected float HealthMin = 0;
        [SerializeField] protected float HealthMultiplier = 1;

        [Header("Damage")]
        [SerializeField] protected float DamageMax = 10;
        [SerializeField] protected float DamageMin = 1;
        [SerializeField] protected float DamageMultiplier = 1;

        [Header("Attack")]
        [SerializeField] protected bool IsAttacking = false;
        [SerializeField] protected float GlobalCooldown = 1;
        [SerializeField] protected GameObject DamageTakenCountPrefab;
        [SerializeField] protected Vector3 PrefabOffset;
        protected float globalCooldownPeriod = 0;

        [Header("Animation")]
        [SerializeField] protected Animator animationController;

        public delegate void Death();
        public static event Death OnDeath;

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

        public virtual void Attack(params ICharacter[] characters)
        {
            if(!IsAttacking) return;

            for(int charIndex = 0; charIndex < characters.Length; charIndex++)
            {
                float takenDamage = TotalMaxDamage;
                ICharacter character = characters[charIndex];
                character.TakeDamage(takenDamage);
            }
        }

        public virtual float TakeDamage(float damage)
        {
            Debug.Log(gameObject.name + " took damage: " + damage, gameObject);

            if (DamageTakenCountPrefab != null) {
                GameObject obj = Instantiate(DamageTakenCountPrefab, transform.position + PrefabOffset, transform.rotation);
                NumberPopup popup = obj.GetComponent<NumberPopup>();
                popup.SetNumberText(damage);
            }
            float newHp = Health - damage;
            if (newHp <= 0) OnDeath();

            return SetHealth(Health - damage);
        }

        protected virtual void SetIsAttacking()
        {
            IsAttacking = true;
        }
        protected virtual void SetIsNotAttacking()
        {
            IsAttacking = false;
        }
    }
}
