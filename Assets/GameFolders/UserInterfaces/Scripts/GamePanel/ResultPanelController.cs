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
        [SerializeField] private GameObject frontPanel;
        [SerializeField] private Image resultBackgroundImage;
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
            frontPanel.transform.localScale = Vector3.zero;
            frontPanel.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce);
            resultPanel.SetActive(true);
            resultText.text = $"CONGRATS! \n YOUR SCORE \n {score}";
            
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
            GameManager.Instance.LastGameState = GameState.Lose;
            resultBackgroundImage.enabled = false;
            resultText.color = new Color(1f, 0.33f, 0.33f);
            frontPanel.transform.localScale = Vector3.zero;
            frontPanel.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce);
            resultPanel.SetActive(true);
            resultText.text = $"FAILED! \n REMAINING MOVE \n OVER!";
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(backgroundRectTransform);

            fillBarImage.DOFillAmount(0f, 5f).SetEase(Ease.OutFlash);

            GameManager.Instance.LoadMenuScene();
        }
        
    }
}
