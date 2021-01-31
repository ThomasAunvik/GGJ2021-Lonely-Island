using LonelyIsland.Characters;
using LonelyIsland.System;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LonelyIsland.UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button ContinueButton;
        [SerializeField] private Button NewGameButton;
        [SerializeField] private TMPro.TMP_Text welcomeText;

        [SerializeField] private string welcomeString;
        [SerializeField] private string diedString;

        [SerializeField] private Color blackColor = Color.black;
        [SerializeField] private Color deadColor = Color.red;

        [SerializeField] private GameUI gameUI;
        [SerializeField] private User user;
        [SerializeField] private Camera viewCamera;
        [SerializeField] private Camera mainCamera;

        private void Awake()
        {
            if (!GameManager.Instance) return;

            ContinueButton.onClick.AddListener(OnContinueButtonClick);
            NewGameButton.onClick.AddListener(OnNewGameButtonClick);

            bool viewUI = (!GameManager.Instance.Teleporting && GameManager.Instance.GetIsFirstSave()) || user.IsDead();

            gameObject.SetActive(viewUI);
            gameUI.gameObject.SetActive(!viewUI);
            user.gameObject.SetActive(!viewUI);

            viewCamera.gameObject.SetActive(viewUI);
            mainCamera.gameObject.SetActive(!viewUI);
        }

        private void Update()
        {
            if(!GameManager.Instance) return;

            if (!GameManager.Instance.firstSave || user.IsDead())
            {
                ContinueButton.gameObject.SetActive(false);
            }

            welcomeText.text = user.IsDead() ? diedString : welcomeString;
            welcomeText.color = user.IsDead() ? deadColor : blackColor;

            if (user.IsDead())
            {
                gameObject.SetActive(true);
                gameUI.gameObject.SetActive(false);
                user.gameObject.SetActive(false);

                viewCamera.gameObject.SetActive(true);
                mainCamera.gameObject.SetActive(false);
            }
        }

        private void OnNewGameButtonClick()
        {
            gameObject.SetActive(false);
            gameUI.gameObject.SetActive(true);

            GameManager.Instance.firstSave = false;

            GameManager.Instance.NewGame();
        }

        private void OnContinueButtonClick()
        {
            gameObject.SetActive(false);
            gameUI.gameObject.SetActive(true);

            viewCamera.gameObject.SetActive(false);
            mainCamera.gameObject.SetActive(true);
            user.gameObject.SetActive(true);

            GameManager.Instance.firstSave = false;
            GameManager.Instance.resetSave = false;
        }
    }
}