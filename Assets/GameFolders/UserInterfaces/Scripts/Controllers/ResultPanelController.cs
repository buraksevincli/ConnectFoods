using ConnectedFoods.Core;
using TMPro;
using UnityEngine;

namespace ConnectedFoods.UserInterface
{
    public class ResultPanelController : MonoBehaviour
    {
        [SerializeField] private GameObject resultPanel;
        [SerializeField] private TMP_Text resultText;
        [SerializeField] private TMP_Text scoreCountText;

        private int _score;

        private void OnEnable()
        {
            DataManager.Instance.EventData.OnWinCondition += WinConditionHandler;
            DataManager.Instance.EventData.OnLoseCondition += LoseConditionHandler;
        }

        private void OnDisable()
        {
            DataManager.Instance.EventData.OnWinCondition -= WinConditionHandler;
            DataManager.Instance.EventData.OnLoseCondition -= LoseConditionHandler;
        }

        private void WinConditionHandler(int score)
        {
            _score = score;
            resultPanel.SetActive(true);
            resultText.text = "Tebrikler !";
            scoreCountText.text = _score.ToString();
        }

        private void LoseConditionHandler(int score)
        {
            _score = score;
            resultPanel.SetActive(true);
            resultText.text = "Başaramadık..";
            scoreCountText.text = _score.ToString();
        }

    }
}
