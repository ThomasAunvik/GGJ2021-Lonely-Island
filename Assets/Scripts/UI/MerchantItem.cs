using LonelyIsland.NPC;
using LonelyIsland.System;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace LonelyIsland.UI
{
    public class MerchantItem : MonoBehaviour
    {
        [Header("Button")]
        [SerializeField] private Button button;
        [SerializeField] private Image buttonImage;
        [SerializeField] private TMPro.TMP_Text itemText;
        [SerializeField] private TMPro.TMP_Text costText;

        [SerializeField] private Color buttonSelectedColor;
        [SerializeField] private Color buttonNotSelectedColor;
        [SerializeField] private Color buttonNotAffordSelectedColor;
        [SerializeField] private Color buttonNotAffordColor;

        [Header("Control")]
        [SerializeField] private Image selectedImage;
        [SerializeField] private Sprite FKey;
        [SerializeField] private Sprite AButton;

        private bool isSelected = false;

        [Header("Purchase")]
        [SerializeField] private PurchasableItem item;
        public void SetItem(PurchasableItem item) { this.item = item; }
        public PurchasableItem GetItem() { return item; }

        private PlayerInput playerInput;

        private void Awake()
        {
            playerInput = GetComponentInParent<PlayerInput>();
        }

        private void Update()
        {
            if (!GameManager.Instance) return;
            if (item == null) return;

            costText.text = item.cost.ToString();
            itemText.text = "Buy " + item.itemName;

            bool canAfford = GameManager.Instance.Save.Coins - item.cost >= 0;
            selectedImage.gameObject.SetActive(isSelected && canAfford);

            button.enabled = canAfford;
            buttonImage.color = isSelected ?
                (canAfford ? buttonSelectedColor : buttonNotAffordSelectedColor) :
                (canAfford ? buttonNotSelectedColor : buttonNotAffordColor);
            ;

            bool isKeyboard = playerInput.currentControlScheme.Contains("Keyboard");
            selectedImage.sprite = isKeyboard ? FKey : AButton;
        }

        public void SetIsSelected(bool isSelected)
        {
            this.isSelected = isSelected;
        }

        public void SetSelectImageSprite(Sprite image)
        {
            selectedImage.sprite = image;
        }
    }
}