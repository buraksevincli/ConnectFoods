using System;
using System.Collections;
using System.Collections.Generic;
using ConnectedFoods.Backend;
using PlayFab.GroupsModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ConnectedFoods.UserInterface
{
    public class UIClanController : MonoBehaviour
    {
        [Header("UI CLAN SETTINGS")]
        [SerializeField] private TMP_Text clanNameText;
        [SerializeField] private Button applyToClanButton;

        private EntityKey _clanEntityKey;
        private string _clanName;

        private void OnEnable()
        {
            applyToClanButton.onClick.AddListener(ApplyToClanButtonOnClick);
        }
        
        private void OnDisable()
        {
            applyToClanButton.onClick.AddListener(ApplyToClanButtonOnClick);
        }
        
        private void ApplyToClanButtonOnClick()
        {
            PlayFabClanManager.Instance.JoinClan(_clanName , _clanEntityKey);
        }

        public void InitializeSettings(string clanName , EntityKey clanEntityKey)
        {
            _clanName = clanName;
            clanNameText.text = _clanName;
            _clanEntityKey = clanEntityKey;
        }
    }
}

