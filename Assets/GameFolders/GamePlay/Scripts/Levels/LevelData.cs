using UnityEngine;

namespace ConnectedFoods.Data
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Data/Level Data")]
    public class LevelData : ScriptableObject
    {
        [Header("Level Settings")] 
        [SerializeField] private int[] levelRemainingMove;
        [SerializeField] private int[] levelRequiredApple;
        [SerializeField] private int[] levelRequiredBanana;
        [SerializeField] private int[] levelRequiredBlob;
        [SerializeField] private int[] levelRequiredBlueberries;
        [SerializeField] private int[] levelRequiredDragonFruit;
        [SerializeField] private int[] levelHighScore;

        public int[] LevelRemainingMove => levelRemainingMove;
        public int[] LevelRequiredApple => levelRequiredApple;
        public int[] LevelRequiredBanana => levelRequiredBanana;
        public int[] LevelRequiredBlob => levelRequiredBlob;
        public int[] LevelRequiredBlueberries => levelRequiredBlueberries;
        public int[] LevelRequiredDragonFruit => levelRequiredDragonFruit;

        public int[] LevelHighScore
        {
            get => levelHighScore;
            set => levelHighScore = value;
        }
    }
}
