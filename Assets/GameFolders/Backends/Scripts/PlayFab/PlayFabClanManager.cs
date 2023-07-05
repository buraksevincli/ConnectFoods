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
        public static EntityKey EntityKeyMaker(string entityId)
        {
            return new EntityKey { Id = entityId };
        }

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
            
            PlayFabGroupsAPI.CreateGroup(createGroupRequest , OnCreateGroupSuccess, OnCreateGroupError);

        }

        private void OnCreateGroupSuccess(CreateGroupResponse obj)
        {
            Debug.Log("Clan Kuruldu");
        }

        private void OnCreateGroupError(PlayFabError obj)
        {
            
        }

        #endregion

        public void ListGroups()
        {
            ListMembershipRequest listMembershipRequest = new ListMembershipRequest();
            
            PlayFabGroupsAPI.ListMembership(listMembershipRequest , OnListMembershipSuccess , OnListMembershipError);
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
            }
        }
    }
}
