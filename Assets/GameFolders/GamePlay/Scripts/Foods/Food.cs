using System;
using ConnectedFoods.Game;
using UnityEngine;

namespace ConnectedFoods.Data
{
    [Serializable]
    public struct Food
    {
        [SerializeField] private FoodType foodType;
        [SerializeField] private Sprite sprite;

        public FoodType FoodType => foodType;
        public Sprite Sprite => sprite;
    }
}
