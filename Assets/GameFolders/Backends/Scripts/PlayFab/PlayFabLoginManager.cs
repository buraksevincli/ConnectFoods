using System;
using ConnectedFoods.Core;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;


namespace ConnectedFoods.Network
{
    public class PlayFabLoginManager : MonoSingleton<PlayFabLoginManager>
    {
        [SerializeField] private GameObject usernamePanel;
        [SerializeField] private Button levelsButton;
        [SerializeField] private Button chatButton;
        
        public string UsernamePlayerPrefs
        {
            get => PlayerPrefs.GetString(GameConst.PLAYER_USERNAME);
            set => PlayerPrefs.SetString(GameConst.PLAYER_USERNAME, value);
        }

        public string EntityId
        {
            get => PlayerPrefs.GetString(GameConst.ENTITY_ID);
            set => PlayerPrefs.SetString(GameConst.ENTITY_ID, value);
        }

        private void Start()
        {
            if (string.IsNullOrEmpty(UsernamePlayerPrefs))
            {
                usernamePanel.gameObject.SetActive(true);
                levelsButton.interactable = false;
                chatButton.interactable = false;
                return;
            }
            
            usernamePanel.gameObject.SetActive(false);
            levelsButton.interactable = true;
            chatButton.interactable = true;
 
            LoginWithPlayFabAccount();
        }

        private void LoginWithPlayFabAccount()
        {
            LoginWithPlayFabRequest loginRequest = new LoginWithPlayFabRequest()
            {
                Username = UsernamePlayerPrefs,
                Password = GameConst.DEFAULT_PLAYER_PASSWORD
            };

            PlayFabClientAPI.LoginWithPlayFab
            (
                loginRequest,
                result =>
                {
                    DataManager.Instance.EventData.OnLoginSuccess?.Invoke();
                    EntityId = result.EntityToken.Entity.Id;
                },
                error =>
                {
                    DataManager.Instance.EventData.OnLoginError?.Invoke();
                }
            );
        }

        public void RegisterWithPlayFabID(string username, Action<string> onLoginErrorAction, Action<string> onLoginSuccess)
        {
            RegisterPlayFabUserRequest registerRequest = new RegisterPlayFabUserRequest()
            {
                Username = username,
                Password = GameConst.DEFAULT_PLAYER_PASSWORD,
                RequireBothUsernameAndEmail = false
            };

            PlayFabClientAPI.RegisterPlayFabUser
            (
                registerRequest,
                result =>
                {
                    UsernamePlayerPrefs = username;
                    SetDisplayName();
                    LoginWithPlayFabAccount();
                    DataManager.Instance.EventData.OnSetUsername?.Invoke(UsernamePlayerPrefs);
                    onLoginSuccess?.Invoke("Register Success !");
                },
                error => { onLoginErrorAction?.Invoke(error.ErrorMessage); }
            );
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void SetDisplayName()
        {
            UpdateUserTitleDisplayNameRequest displayNameRequest = new UpdateUserTitleDisplayNameRequest()
            {
                DisplayName = UsernamePlayerPrefs
            };

            PlayFabClientAPI.UpdateUserTitleDisplayName
            (
                displayNameRequest,
                result => {  },
                error => {  }
            );
        }
    }
}
