using System.Collections.Generic;
using UnityEngine;

namespace ConnectedFoods.Game
{
    public class GridNode
    {
        public List<GridNode> Neighbors { get; set; } = new List<GridNode>();
        public FoodItem FoodItem { get; set; }
        public Vector2 Position { get; set;}
        public bool IsEmpty { get; set; }
    }
}
