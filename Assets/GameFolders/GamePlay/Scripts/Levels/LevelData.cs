using System;
using System.Linq;
using ConnectedFoods.Core;
using ConnectedFoods.Level;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ConnectedFoods.Data
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Data/Level Data")]
    public class LevelData : ScriptableObject
    {
        [Header("Level Settings")] 
        [SerializeField] private LevelInfo[] levelInfos;

        public int MaxLevel => levelInfos.Length;
        
        public LevelInfo GetLevelInfo(int level)
        {
            return levelInfos[level -1];
        }

        public void SetHighScore(int level, int score)
        {
            int oldScore = PlayerPrefs.GetInt($"Level{level}");
            if (score > oldScore)
            {
                GameManager.Instance.LastGameState = GameState.HighScoreWin;
                PlayerPrefs.SetInt($"Level{level}", score);
            }
            else
            {
                GameManager.Instance.LastGameState = GameState.Win;
            }
        }

        public int GetHighScore(int level)
        {
            return PlayerPrefs.GetInt($"Level{level}");
        }
#if UNITY_EDITOR
        private void OnValidate()
        {
            for (int i = 0; i < levelInfos.Length; i++)
            {
                levelInfos[i].SetLevelID(i + 1);
            }
        }
#endif
    }
}
