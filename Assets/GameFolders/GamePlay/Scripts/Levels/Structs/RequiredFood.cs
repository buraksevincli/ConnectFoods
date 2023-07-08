using System;
using UnityEngine;

namespace ConnectedFoods.Level
{
    [Serializable]
    public struct RequiredFood
    {
        [SerializeField] private FoodType foodType;
        [SerializeField] private int amount;
        
        public FoodType FoodType => foodType;
        public int Amount => amount;
    }
}