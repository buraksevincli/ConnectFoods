using System;
using TMPro;
using UnityEngine;

namespace ConnectedFoods.UserInterface
{
    [Serializable]
    public struct GoalPanelItem
    {
        [SerializeField] private FoodType foodType;
        [SerializeField] private GameObject foodGameObject;
        [SerializeField] private TextMeshProUGUI foodText;

        private int _remainingAmount;
        
        public FoodType FoodType => foodType;
        public GameObject FoodGameObject => foodGameObject;
        public TextMeshProUGUI FoodText => foodText;
        public int RemainingAmount
        {
            get => _remainingAmount;
            set
            {
                _remainingAmount = value < 0 ? 0 : value;
                foodText.text = $"{_remainingAmount}";
            }
        }
    }
}