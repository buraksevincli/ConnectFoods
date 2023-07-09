using System;
using ConnectedFoods.Core;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace ConnectedFoods.Network
{
    public class PlayFabLoginUIController : MonoBehaviour
    {
        [Header("Login Settings")] 
        [SerializeField] private TMP_InputField usernameInputField;
        [SerializeField] private GameObject playFabLoginPanel;
        [SerializeField] private Button usernameConfirmButton;
        [SerializeField] private TMP_Text infoMessage;

        private Action<string> onLoginError;
        private Action<string> onLoginSuccess;

        private void OnEnable()
        {
            usernameConfirmButton.onClick?.AddListener(UsernameConfirmButtonOnClick);
            usernameInputField.onValueChanged?.AddListener(UsernameInputFieldOnValueChanged);
            onLoginError += OnLoginErrorTextUpdate;
            onLoginSuccess += OnLoginSuccessTextUpdate;
        }

        private void OnDisable()
        {
            usernameConfirmButton.onClick?.RemoveListener(UsernameConfirmButtonOnClick);
            usernameInputField.onValueChanged?.AddListener(UsernameInputFieldOnValueChanged);
            onLoginError -= OnLoginErrorTextUpdate;
            onLoginSuccess -= OnLoginSuccessTextUpdate;
        }

        private void UsernameInputFieldOnValueChanged(string inputField)
        {
            infoMessage.text = "";
            usernameConfirmButton.interactable = usernameInputField.text.Length > GameConst.MIN_USERNAME_LENGHT;
        }
        
        private void UsernameConfirmButtonOnClick()
        {
            PlayFabLoginManager.Instance.RegisterWithPlayFabID(usernameInputField.text , onLoginError, onLoginSuccess);
        }
        
        private void OnLoginErrorTextUpdate(string message)
        {
            infoMessage.text = message;
        }
        
        private void OnLoginSuccessTextUpdate(string message)
        {
            infoMessage.text = message;
        }
    }
}

