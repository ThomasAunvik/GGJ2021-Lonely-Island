using LonelyIsland.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LonelyIsland.Character
{
    public class User : Character
    {
        protected override float SetHealth(float newHealth) { return health = Mathf.Clamp(newHealth, HealthMin, TotalMaxHealth); }
        public override float Damage { get { return GameManager._instance.Stats.Damage * DamageMultiplier; } }
        public override float TotalMaxHealth { get { return GameManager._instance.Stats.Health * HealthMultiplier * health; } }
        public override float TotalMaxDamage { get { return GameManager._instance.Stats.Damage * DamageMultiplier; } }

        private void Start()
        {
            SetHealth(TotalMaxHealth);
        }
    }
}
