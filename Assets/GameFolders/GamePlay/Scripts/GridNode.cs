using UnityEngine;

namespace ConnectedFoods.Game
{
    public class GridNode
    {
        public FoodItem FoodItem { get; set; }
        public Vector2 Position { get; set;}
        public bool IsEmpty { get; set; }
    }
}
