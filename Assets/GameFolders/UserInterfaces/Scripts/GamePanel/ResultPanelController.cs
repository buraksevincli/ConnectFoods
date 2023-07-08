using System;
using ConnectedFoods.Core;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ConnectedFoods.UserInterface
{
    public class ResultPanelController : MonoBehaviour
    {
        [SerializeField] private GameObject resultPanel;
        [SerializeField] private TMP_Text resultText;
        [SerializeField] private RectTransform backgroundRectTransform;
        [SerializeField] private Image fillBarImage;

        private void OnEnable()
        {
            DataManager.Instance.EventData.OnWinCondition += WinConditionHandler;
            DataManager.Instance.EventData.OnLoseCondition += LoseConditionHandler;
        }

        private void OnDisable()
        {
            DataManager.Instance.EventData.OnLoseCondition -= LoseConditionHandler;
            DataManager.Instance.EventData.OnWinCondition -= WinConditionHandler;
        }

        private void WinConditionHandler(int score)
        {
            resultPanel.SetActive(true);
            resultText.text = $"TEBRIKLER ! \n SKORUNUZ \n {score}";
            
            if (GameManager.Instance.SelectedLevel == GameManager.Instance.Level)
            {
                GameManager.Instance.Level += 1;
            }
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(backgroundRectTransform);
            
            DataManager.Instance.LevelData.SetHighScore(GameManager.Instance.SelectedLevel, score);

            fillBarImage.DOFillAmount(0f, 5f);
            
            GameManager.Instance.LoadMenuScene();
        }

        private void LoseConditionHandler(int score)
        {
            resultPanel.SetActive(true);
            resultText.text = $"BASARAMADIK ! \n SKORUNUZ \n {score}";
            
            fillBarImage.DOFillAmount(0f, 5f).SetEase(Ease.OutFlash);

            GameManager.Instance.LoadMenuScene();
        }
        
    }
}
