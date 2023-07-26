using System;
using ConnectedFoods.Core;
using DG.Tweening;
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
        [SerializeField] private Color activeLevelColor;
        [SerializeField] private Color deActiveColor;

        public void Initiate(int level, LevelStatus status, Action<int> onClick, ref Action newLevelAction, ref Action openButtonAction)
        {
            transform.localScale = Vector3.zero;
            button.onClick.AddListener(()=> onClick(level));
            levelText.text = $"Level {level}";
            scoreText.text = $"Score {DataManager.Instance.LevelData.GetHighScore(level)}";

            openButtonAction += () => transform.DOScale(Vector3.one, 1f).SetEase(Ease.OutBack);

            switch (status)
            {
                case LevelStatus.Active:
                    button.interactable = true;
                    button.image.color = activeLevelColor;
                    break;
                case LevelStatus.Current:
                    button.interactable = true;
                    if (GameManager.Instance.IsOpenNewLevel)
                    {
                        newLevelAction += () => button.image.DOColor(activeLevelColor, 1);
                    }
                    else
                    {
                        button.image.color = activeLevelColor;
                    }
                    break;
                case LevelStatus.Deactivate:
                    button.interactable = false;
                    button.image.color = deActiveColor;
                    break;
            }
        }
    }
}
