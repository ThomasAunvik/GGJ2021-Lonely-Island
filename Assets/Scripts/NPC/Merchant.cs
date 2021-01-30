using LonelyIsland.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using LonelyIsland.UI;
using System.Linq;
using LonelyIsland.System;
using System;

namespace LonelyIsland.NPC
{
    public class Merchant : MonoBehaviour
    {
        [SerializeField] private Canvas merchantCanvas;
        [SerializeField] private float viewUIRange = 10;
        [SerializeField] private PlayerInput playerInput;

        [SerializeField] private List<PurchasableItem> purchasableItems;
        [SerializeField] private GameObject itemPrefab;
        [SerializeField] private Transform spawn;

        private List<MerchantItem> SelectablePurchases = new List<MerchantItem>();
        private int selectedPurchase = 0;

        private User player;

        private void Awake()
        {
            player = FindObjectOfType<User>();

            GenerateItems();
        }

        public void GenerateItems()
        {
            if (!GameManager.Instance) return;

            foreach (Transform child in spawn)
            {
                Destroy(child.gameObject);
            }

            SelectablePurchases.Clear();

            purchasableItems.Sort((a, b) =>
                Enum.GetName(typeof(PurchasableItemTypes), a.itemType).CompareTo(
                    Enum.GetName(typeof(PurchasableItemTypes), b.itemType)
                )
            );

            for (int i = 0; i < purchasableItems.Count; i++)
            {
                if (!purchasableItems[i].MeetsRequirements()) { continue; }

                GameObject obj = Instantiate(itemPrefab, spawn);
                MerchantItem item = obj.GetComponent<MerchantItem>();
                item.SetItem(purchasableItems[i]);
                SelectablePurchases.Add(item);
            }

            if (SelectablePurchases.Count > 0)
            {
                if (selectedPurchase >= SelectablePurchases.Count)
                    selectedPurchase = SelectablePurchases.Count - 1;

                SelectablePurchases[selectedPurchase].SetIsSelected(true);
            }
        }

        private void Update()
        {
            if (!SelectablePurchases.All(x => purchasableItems.Any(y => y.itemId == x.GetItem().itemId && y.MeetsRequirements())))
            {
                GenerateItems();
            }

            bool isInRange = (transform.position - player.transform.position).magnitude <= viewUIRange;
            merchantCanvas.gameObject.SetActive(isInRange);
            playerInput.enabled = isInRange;
        }

        public void OnScroll(InputAction.CallbackContext ctx)
        {
            var value = ctx.ReadValue<float>();

            if (value == 0) return;
            int newIndex = selectedPurchase + (value > 0 ? 1 : -1);
            if (newIndex >= SelectablePurchases.Count) newIndex = 0;
            else if (newIndex < 0) newIndex = SelectablePurchases.Count - 1;

            if (SelectablePurchases[selectedPurchase] != null)
                SelectablePurchases[selectedPurchase].SetIsSelected(false);

            selectedPurchase = newIndex;
            var newSelected = SelectablePurchases[newIndex];
            newSelected.SetIsSelected(true);
        }

        public void OnBuy(InputAction.CallbackContext ctx)
        {
            var input = ctx.ReadValue<float>();
            if (input <= 0) return; 

            if (SelectablePurchases.Count >= selectedPurchase)
            {
                var purchased = SelectablePurchases[selectedPurchase];
                purchasableItems.Find(x => x.itemId == purchased.GetItem().itemId)?.Purchase();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, viewUIRange);   
        }

        public void ControlsChanged(PlayerInput playerInput)
        {
            Debug.Log("Changed to: " + playerInput.name);
        }
    }
}