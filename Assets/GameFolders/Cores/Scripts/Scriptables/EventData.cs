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
        public Action OnLoginSuccess { get; set; }
        public Action OnLoginError { get; set; }
        
        public Action<FoodItem> OnSelectFoodItem { get; set; }
        public Action<FoodType, int> OnMatch { get; set; }
        public Action<int> OnWinCondition { get; set; }
        public Action<int> OnLoseCondition { get; set; }
    }
}
