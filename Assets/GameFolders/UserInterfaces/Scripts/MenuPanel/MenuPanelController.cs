using System;
using ConnectedFoods.Core;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace ConnectedFoods.UserInterface
{
    public class MenuPanelController : MonoBehaviour
    {
        [SerializeField] private Button chatButton;
        [SerializeField] private GameObject chatRoom;

        [SerializeField] private Button levelsButton;
        [SerializeField] private GameObject levelsPanel;
        
        private void OnEnable()
        {
            DataManager.Instance.EventData.OnLoginSucces += OnChatPanelOpen;
            chatButton.onClick?.AddListener(ChatButtonOnClick);
            levelsButton.onClick?.AddListener(LevelsButtonOnClick);
        }

        private void OnDisable()
        {
            DataManager.Instance.EventData.OnLoginSucces -= OnChatPanelOpen;
            chatButton.onClick?.RemoveListener(ChatButtonOnClick);
            levelsButton.onClick.RemoveListener(LevelsButtonOnClick);
        }
        
        private void OnChatPanelOpen()
        {
            chatButton.gameObject.SetActive(true);
        }

        private void ChatButtonOnClick()
        {
            chatRoom.SetActive(!chatRoom.activeSelf);
            levelsButton.gameObject.SetActive(!levelsButton.gameObject.activeSelf);
        }

        private void LevelsButtonOnClick()
        {
            levelsPanel.SetActive(!levelsPanel.activeSelf);
        }

        private void ExitButtonOnClick()
        {
            Application.Quit();
        }
    }
}
