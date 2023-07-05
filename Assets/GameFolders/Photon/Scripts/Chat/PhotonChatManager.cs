using System;
using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameFolders.Photon.Scripts.Chat
{
    public class PhotonChatManager : MonoBehaviour, IChatClientListener
    {
        [SerializeField] private string username;

        [SerializeField] private TMP_InputField messageInput;

        [SerializeField] private TMP_Text chatDisplay;

        [SerializeField] private GameObject joinPanel;
        [SerializeField] private GameObject chatPanel;
        
        private ChatClient _chatClient;

        private string _currentChat;
        private string _privateReceiver = "";
        
        private bool _isConnected;

        private void Update()
        {
            if (_isConnected)
            {
                _chatClient.Service();
            }
            
            if (messageInput.text != "" && Input.GetKey(KeyCode.Return))
            {
                SubmitPublicChatOnClick();
                SubmitPrivateChatOnClick();
            }
        }

        #region Setup

        public void UsernameOnValueChange(string valueIn)
        {
            username = valueIn;
        }

        public void ChatConnectOnClick()
        {
            _isConnected = true;

            _chatClient = new ChatClient(this);
            
            _chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion,
                new AuthenticationValues(username));
            
        }

        #endregion

        #region PublicChat

        public void SubmitPublicChatOnClick()
        {
            if (_privateReceiver == "")
            {
                _chatClient.PublishMessage("RegionChannel", _currentChat);
                messageInput.text = "";
                _currentChat = "";
            }
        }

        public void TypeChatOnValueChange(string valueIn)
        {
            _currentChat = valueIn;
        }

        #endregion

        #region PrivateChat

        public void SubmitPrivateChatOnClick()
        {
            if (_privateReceiver != "")
            {
                _chatClient.SendPrivateMessage(_privateReceiver, _currentChat);
                messageInput.text = "";
                _currentChat = "";
            }
        }

        public void ReceiverOnValueChange(string valueIn)
        {
            _privateReceiver = valueIn;
        }

        #endregion
        

        public void DebugReturn(DebugLevel level, string message)
        {
            
        }

        public void OnDisconnected()
        {
        }

        public void OnConnected()
        {
            joinPanel.SetActive(false);
            _chatClient.Subscribe(new string[] { "RegionChannel" });
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
            chatPanel.SetActive(true);
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