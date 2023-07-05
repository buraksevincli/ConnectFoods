using ConnectedFoods.Core;
using ConnectedFoods.Network;
using PlayFab;
using PlayFab.GroupsModels;
using UnityEngine;
using EntityKey = PlayFab.GroupsModels.EntityKey;

namespace ConnectedFoods.Backend
{
    public class PlayFabClanManager : MonoSingleton<PlayFabClanManager>
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                ListGroups();
            }
        }

        #region Create Clan

        public void CreateClan(string groupName)
        {
            CreateGroupRequest createGroupRequest = new CreateGroupRequest()
            {
                GroupName = groupName
            };

            PlayFabGroupsAPI.CreateGroup(createGroupRequest, OnCreateGroupSuccess, OnCreateGroupError);
        }

        private void OnCreateGroupSuccess(CreateGroupResponse obj)
        {
            Debug.Log("Clan Kuruldu");
        }

        private void OnCreateGroupError(PlayFabError obj)
        {
        }

        #endregion

        #region List Clans

        public void ListGroups()
        {
            ListMembershipRequest listMembershipRequest = new ListMembershipRequest();

            PlayFabGroupsAPI.ListMembership(listMembershipRequest, OnListMembershipSuccess, OnListMembershipError);
        }

        private void OnListMembershipError(PlayFabError obj)
        {
            Debug.Log(obj.ErrorMessage);
        }

        private void OnListMembershipSuccess(ListMembershipResponse result)
        {
            foreach (var clan in result.Groups)
            {
                Debug.Log(clan.GroupName + "  " + clan.Group);
                DataManager.Instance.EventData.OnCreateClanObject?.Invoke(clan.GroupName, clan.Group);
            }
        }

        #endregion

        public void JoinClan(string clanName, EntityKey clanEntityKey)
        {
            ApplyToGroupRequest applyToGroupRequest = new ApplyToGroupRequest()
            {
                Group = clanEntityKey,
                AutoAcceptOutstandingInvite = true
            };

            PlayFabGroupsAPI.ApplyToGroup(applyToGroupRequest, 
                response => { Debug.Log("Clana Girildi");},
                error => { Debug.Log(error.ErrorMessage);});
        }
    }
}