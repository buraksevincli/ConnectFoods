using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ConnectedFoods.Level
{
    [Serializable]
    public struct LevelInfo
    {
        [SerializeField, ReadOnly] private int level;
        [SerializeField, Range(4, 9)] private int size;
        [SerializeField] private int remainingMove;
        [SerializeField] private RequiredFood[] requiredFoods;

        public int Level => level;
        public int Size => size;
        public int RemainingMove => remainingMove;
        public RequiredFood[] RequiredFoods => requiredFoods;
        
        public void SetLevelID(int levelID)
        {
            level = levelID;
        }
    }
}