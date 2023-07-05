using System.Linq;
using UnityEngine;

namespace ConnectedFoods.Data
{
    [CreateAssetMenu(fileName = "FoodData", menuName = "Data/Food Data")]
    public class FoodData : ScriptableObject
    {
        [SerializeField] private Food[] foods;

        public Sprite GetFoodSprite(FoodType foodType)
        {
           return foods.First(food => food.FoodType == foodType).Sprite;
        }
    }
}
