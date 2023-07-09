using System;
using ConnectedFoods.Core;
using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace ConnectedFoods.Network
{
    public class PhotonChatManager : MonoBehaviour, IChatClientListener
    {
        [Header("Chat Message Settings")]
        [SerializeField] private TMP_InputField messageInput;
        [SerializeField] private TMP_InputField privateReceiver;
        [SerializeField] private TMP_Text chatDisplay;
        [SerializeField] private Button sendMessageButton;
        
        private ChatClient _chatClient;
        
        private bool _isConnected;

        private void OnEnable()
        {
            DataManager.Instance.EventData.OnLoginSuccess += ChatConnectOnClick;
            sendMessageButton.onClick.AddListener(SendMessageButtonOnClick);
        }

        private void Update()
        {
            if (_isConnected)
            {
                _chatClient.Service();
            }
        }

        private void OnDisable()
        {
            DataManager.Instance.EventData.OnLoginSuccess -= ChatConnectOnClick;
            sendMessageButton.onClick.RemoveListener(SendMessageButtonOnClick);
        }
        
        #region Setup

        private void ChatConnectOnClick()
        {
            _isConnected = true;

            _chatClient = new ChatClient(this);
            
            _chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion,
                new AuthenticationValues(PlayFabLoginManager.Instance.UsernamePlayerPrefs));
        }

        #endregion

        private void SendMessageButtonOnClick()
        {
            if (string.IsNullOrEmpty(privateReceiver.text))
            {
                _chatClient.PublishMessage("GlobalChannel", messageInput.text);
                messageInput.text = "";
            }
            else
            {
                if (privateReceiver.text.Length < 3)
                {
                    return;
                }
                
                _chatClient.SendPrivateMessage(privateReceiver.text, messageInput.text);
                messageInput.text = "";
            }
        }
        
        public void DebugReturn(DebugLevel level, string message)
        {
            
        }

        public void OnDisconnected()
        {
        }

        public void OnConnected()
        {
            _chatClient.Subscribe(new string[] { "GlobalChannel" });
        }

        public void OnChatStateChange(ChatState state)
        {
            
        }

        public void OnGetMessages(string channelName, string[] senders, object[] messages)
        {
            string publicMessage;

            for (int i = 0; i < senders.Length; i++)
            {
                publicMessage = string.Format("{0}: {1}", senders[i], messages[i]);
                chatDisplay.text += "\n " + publicMessage;
            }
        }

        public void OnPrivateMessage(string sender, object message, string channelName)
        {
            string privateMessage;

            privateMessage = string.Format("(Private) {0}: {1}", sender, message);

            chatDisplay.text += "\n " + privateMessage;
        }

        public void OnSubscribed(string[] channels, bool[] results)
        {
            
        }

        public void OnUnsubscribed(string[] channels)
        {
        }

        public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
        {
        }

        public void OnUserSubscribed(string channel, string user)
        {
        }

        public void OnUserUnsubscribed(string channel, string user)
        {
        }
    }
}
