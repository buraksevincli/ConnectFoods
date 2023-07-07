using System;
using UnityEngine;

namespace ConnectedFoods.Core
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public int Level { get; set; }

        private void Start()
        {
            Level = 1;
        }
    }
}
