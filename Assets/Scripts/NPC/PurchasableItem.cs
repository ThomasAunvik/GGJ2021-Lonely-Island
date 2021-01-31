using LonelyIsland.System;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LonelyIsland.NPC
{
    public enum PurchasableItemTypes
    {
        HP, DPS
    }

    [CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/New Item", order = 1)]
    public class PurchasableItem : ScriptableObject
    {
        public int itemId;
        public string itemName;
        public string itemDescription;

        public PurchasableItemTypes itemType;
        public int amount;
        public int cost;

        public bool buyOnce;

        public PurchasableItem[] preRequirements;

        public bool MeetsRequirements()
        {
            if (!GameManager.Instance) return false;

            if (buyOnce)
                if (GameManager.Instance.Save.PurchasedItems.Any(x => x == itemId))
                    return false;
            if (preRequirements.Length == 0) return true;

            return preRequirements.All(x => GameManager.Instance.Save.PurchasedItems.Any(y => x.itemId == y));
        }

        public void Purchase()
        {
            if (!GameManager.Instance) return;
            if (!GameManager.Instance.UseMoney(cost))
            {
                return;
            }

            Debug.Log("Purchased: " + Enum.GetName(typeof(PurchasableItemTypes), itemType) + " for " + cost + " coins.");

            GameManager.Instance.Save.PurchasedItems.Add(itemId);

            switch (itemType)
            {
                case PurchasableItemTypes.HP:
                    GameManager.Instance.Stats.Health += amount;
                    break;
                case PurchasableItemTypes.DPS:
                    GameManager.Instance.Stats.Damage += amount;
                    break;
            }
        }
    }
}
