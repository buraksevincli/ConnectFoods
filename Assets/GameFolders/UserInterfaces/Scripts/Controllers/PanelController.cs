using System;
using ConnectedFoods.Core;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace ConnectedFoods.UserInterface
{
    public class PanelController : MonoBehaviour
    {
        [Header("Chat Settings")]
        [SerializeField] private GameObject chatRoom;
        [SerializeField] private Button chatButton;

        private void OnEnable()
        {
            DataManager.Instance.EventData.OnLoginSucces += OnChatPanelOpen;
            chatButton.onClick?.AddListener(ChatButtonOnClick);
        }

        private void OnDisable()
        {
            chatButton.onClick?.RemoveListener(ChatButtonOnClick);
            DataManager.Instance.EventData.OnLoginSucces -= OnChatPanelOpen;
        }
        
        private void OnChatPanelOpen()
        {
            chatButton.gameObject.SetActive(true);
        }

        private void ChatButtonOnClick()
        {
            chatRoom.SetActive(!chatRoom.activeSelf);

        }
    }
}
