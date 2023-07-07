using System;
using ConnectedFoods.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ConnectedFoods.Network
{
    public class PlayFabLoginUIController : MonoBehaviour
    {
        [Header("Login Settings")] 
        [SerializeField] private TMP_InputField usernameInputField;
        [SerializeField] private GameObject playFabLoginPanel;
        [SerializeField] private Button usernameConfirmButton;
        [SerializeField] private TMP_Text errorMessage;

        private Action<string> onLoginError;

        private void OnEnable()
        {
            usernameConfirmButton.onClick?.AddListener(UsernameConfirmButtonOnClick);
            usernameInputField.onValueChanged?.AddListener(UsernameInputFieldOnValueChanged);
            onLoginError += OnLoginErrorTextUpdate;
        }

        private void OnDisable()
        {
            usernameConfirmButton.onClick?.RemoveListener(UsernameConfirmButtonOnClick);
            usernameInputField.onValueChanged?.AddListener(UsernameInputFieldOnValueChanged);
            onLoginError -= OnLoginErrorTextUpdate;
        }

        private void UsernameInputFieldOnValueChanged(string inputField)
        {
            errorMessage.gameObject.SetActive(false);
            usernameConfirmButton.interactable = usernameInputField.text.Length > GameConst.MIN_USERNAME_LENGHT;
        }
        
        private void UsernameConfirmButtonOnClick()
        {
            PlayFabLoginManager.Instance.RegisterWithPlayFabID(usernameInputField.text , onLoginError);
        }
        
        private void OnLoginErrorTextUpdate(string errorMessage)
        {
            this.errorMessage.text = errorMessage;
        }
    }
}

