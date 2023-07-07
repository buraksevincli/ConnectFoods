using System;
using ConnectedFoods.Core;
using ConnectedFoods.UserInterface;
using PlayFab.GroupsModels;
using UnityEngine;

namespace ConnectedFoods.UserInterface
{
    public class ClanScrollViewUIController : MonoBehaviour
    {
        [Header("Scroll View Settings")]
        [SerializeField] private UIClanController uiClan;
        [SerializeField] private GameObject clansParent;

        private void OnEnable()
        {
            DataManager.Instance.EventData.OnCreateClanObject += OnCreateClanObject;
        }

        private void OnDisable()
        {
            DataManager.Instance.EventData.OnCreateClanObject -= OnCreateClanObject;
        }

        private void OnCreateClanObject(string clanName, EntityKey clanEntityKey)
        {
            UIClanController _uıClanController = Instantiate(uiClan, clansParent.transform);
            _uıClanController.InitializeSettings(clanName , clanEntityKey);
        }
    }
}
