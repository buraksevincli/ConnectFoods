using System;
using ConnectedFoods.Game;
using PlayFab.GroupsModels;
using UnityEngine;

namespace ConnectedFoods.Core
{
    [CreateAssetMenu(fileName = "EventData", menuName = "Data/Event Data")]
    public class EventData : ScriptableObject
    {
        public Action<string> OnSetUsername { get; set; }
        public Action OnLoginSucces { get; set; }
        
        public Action<string , EntityKey> OnCreateClanObject { get; set; }
        
        public Action<FoodItem> OnSelectFoodItem { get; set; }
        public Action OnMatch { get; set; }
    }
}
