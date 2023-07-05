using System;
using ConnectedFoods.Core;
using ConnectedFoods.Cores;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;


namespace ConnectedFoods.Network
{
    public class PlayFabLoginManager : MonoSingleton<PlayFabLoginManager>
    {
        [SerializeField] private GameObject usernamePanel;
        public string UsernamePlayerPrefs
        {
            get => PlayerPrefs.GetString(GameConst.PLAYER_USERNAME);
            set => PlayerPrefs.SetString(GameConst.PLAYER_USERNAME, value);
        }

        private void Start()
        {
            if (string.IsNullOrEmpty(UsernamePlayerPrefs))
            {
                usernamePanel.gameObject.SetActive(true);
                return;
            }
            
            usernamePanel.gameObject.SetActive(false);
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
                    DataManager.Instance.EventData.OnLoginSucces?.Invoke();
                },
                error => { }
            );
        }
        
        public void RegisterWithPlayFabID(string username, Action<string> onLoginErrorAction)
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
                    Debug.Log("Success Register");
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
                result => { Debug.Log("Success Display Name"); },
                error => { Debug.Log(error.ErrorMessage); }
            );
        }
    }
}
