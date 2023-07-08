using System;
using ConnectedFoods.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ConnectedFoods.Level
{
    public class LevelButtonItem : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI scoreText;

        public Button Button => button;
        public TextMeshProUGUI LevelText => levelText;
        public TextMeshProUGUI ScoreText => scoreText;

        public void Initiate(int level, LevelStatus status, Action<int> onClick)
        {
            button.onClick.AddListener(()=> onClick(level));
            levelText.text = $"{level}";
            scoreText.text = $"{DataManager.Instance.LevelData.GetHighScore(level)}";

            switch (status)
            {
                case LevelStatus.Active:
                    button.interactable = true;
                    break;
                case LevelStatus.Current:
                    button.interactable = true;
                    break;
                case LevelStatus.Deactivate:
                    button.interactable = false;
                    break;
            }
        }
    }
}
