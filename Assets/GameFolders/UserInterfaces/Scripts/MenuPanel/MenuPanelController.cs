using System;
using ConnectedFoods.Core;
using UnityEngine;
using UnityEngine.UI;

namespace ConnectedFoods.UserInterface
{
    public class MenuPanelController : MonoBehaviour
    {
        [SerializeField] private Button chatButton;
        [SerializeField] private GameObject chatRoom;

        [SerializeField] private Button levelsButton;
        [SerializeField] private GameObject levelsPanel;

        [SerializeField] private Image connectionErrorImage;

        [SerializeField] private GameObject loginPanel;
        
        private void OnEnable()
        {
            DataManager.Instance.EventData.OnLoginSuccess += OnLoginSuccessHandler;
            DataManager.Instance.EventData.OnLoginError += OnLoginErrorHandler;
            chatButton.onClick?.AddListener(ChatButtonOnClick);
            levelsButton.onClick?.AddListener(LevelsButtonOnClick);
        }

        private void OnDisable()
        {
            DataManager.Instance.EventData.OnLoginSuccess -= OnLoginSuccessHandler;
            DataManager.Instance.EventData.OnLoginError -= OnLoginErrorHandler;
            chatButton.onClick?.RemoveListener(ChatButtonOnClick);
            levelsButton.onClick.RemoveListener(LevelsButtonOnClick);
        }

        private void Start()
        {
            switch (GameManager.Instance.LastGameState)
            {
                case GameState.None:
                    break;
                case GameState.Win:
                    LevelsButtonOnClick();
                    break;
                case GameState.HighScoreWin:
                    LevelsButtonOnClick();
                    break;
                case GameState.Lose:
                    break;
            }
        }

        private void OnLoginErrorHandler()
        {
            connectionErrorImage.gameObject.SetActive(true);
        }

        private void OnLoginSuccessHandler()
        {
            connectionErrorImage.gameObject.SetActive(false);
            loginPanel.SetActive(false);
            levelsButton.interactable = true;
            chatButton.interactable = true;
        }

        private void ChatButtonOnClick()
        {
            chatRoom.SetActive(!chatRoom.activeSelf);
            levelsButton.gameObject.SetActive(!levelsButton.gameObject.activeSelf);
        }

        private void LevelsButtonOnClick()
        {
            levelsPanel.SetActive(true);
            levelsButton.gameObject.SetActive(false);
            chatButton.gameObject.SetActive(false);
        }
    }
}
