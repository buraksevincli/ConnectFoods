using System;
using ConnectedFoods.Core;
using TMPro;
using UnityEngine;

namespace ConnectedFoods.UserInterface
{
    public class ResultPanelController : MonoBehaviour
    {
        [SerializeField] private GameObject resultPanel;
        [SerializeField] private TMP_Text resultText;

        private int _score;
        private int _level;

        private void Awake()
        {
            _level = GameManager.Instance.Level - 1;
        }

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
            _score = score;
            resultPanel.SetActive(true);
            resultText.text = $"TEBRIKLER ! \n SKORUNUZ \n {_score}";
            GameManager.Instance.Level += 1;

            if (_score > DataManager.Instance.LevelData.LevelHighScore[_level])
            {
                DataManager.Instance.LevelData.LevelHighScore[_level] = _score;
            }
            
            GameManager.Instance.LoadMenuScene();
        }

        private void LoseConditionHandler(int score)
        {
            _score = score;
            resultPanel.SetActive(true);
            resultText.text = $"BASARAMADIK ! \n SKORUNUZ \n {_score}";
            
            GameManager.Instance.LoadMenuScene();
        }
    }
}
