using LonelyIsland.System;
using UnityEngine;

namespace LonelyIsland.Characters
{
    public class Enemy : Character
    {
        [Header("Rewards")]
        [SerializeField] protected int coinLoot;

        protected override void Died()
        {
            GameManager.Instance.Save.Coins += coinLoot;
            Destroy(gameObject);
        }
    }
}
