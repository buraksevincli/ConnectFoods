using System;
using ConnectedFoods.Backend;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ConnectedFoods.UserInterface
{
    public class ClanUIController : MonoBehaviour
    {
        [Header("Clan Settings")]
        [SerializeField] private Button createClanButton;
        [SerializeField] private TMP_InputField clanNameInputField;

        private void OnEnable()
        {
            createClanButton.onClick.AddListener(CreateClanButtonOnClick);   
        }

        private void OnDisable()
        {
            createClanButton.onClick.AddListener(CreateClanButtonOnClick);   
        }

        private void CreateClanButtonOnClick()
        {
            PlayFabClanManager.Instance.CreateClan(clanNameInputField.text);
        }
    }
}
