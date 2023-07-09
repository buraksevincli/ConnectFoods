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
        [SerializeField] private GameObject confettiPanel;
        
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
                    GameManager.Instance.LastGameState = GameState.None;
                    break;
                case GameState.HighScoreWin:
                    LevelsButtonOnClick();
                    confettiPanel.SetActive(true);
                    GameManager.Instance.LastGameState = GameState.None;
                    break;
                case GameState.Lose:
                    GameManager.Instance.LastGameState = GameState.None;
                    break;
            }
        }

        private void OnLoginErrorHandler()
        {
            chatButton.interactable = false;
            connectionErrorImage.gameObject.SetActive(true);
        }

        private void OnLoginSuccessHandler()
        {
            chatButton.interactable = true;
            connectionErrorImage.gameObject.SetActive(false);
        }

        private void ChatButtonOnClick()
        {
            chatRoom.SetActive(!chatRoom.activeSelf);
            levelsButton.gameObject.SetActive(!levelsButton.gameObject.activeSelf);
        }

        private void LevelsButtonOnClick()
        {
            levelsPanel.SetActive(!levelsPanel.activeSelf);
            levelsButton.gameObject.SetActive(false);
            chatButton.gameObject.SetActive(false);
        }
    }
}
