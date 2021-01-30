using LonelyIsland.System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LonelyIsland.UI {
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text CoinsText;
        [SerializeField] private TMP_Text HPText;

        [SerializeField] private Button SaveButton;
        [SerializeField] private Button ResetButton;

        private void Awake()
        {
            SaveButton.onClick.AddListener(() => GameManager.Instance.SaveGame());
            ResetButton.onClick.AddListener(() => GameManager.Instance.ResetSave());
        }

        void Update()
        {
            if (!GameManager.Instance) return;

            CoinsText.text = GameManager.Instance.Save.Coins.ToString();
            HPText.text = GameManager.Instance.Save.Health.ToString();
        }
    }
}