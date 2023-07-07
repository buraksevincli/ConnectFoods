using ConnectedFoods.Core;
using TMPro;
using UnityEngine;

namespace ConnectedFoods.UserInterface
{
    public class MovePanelController : MonoBehaviour
    {
        [SerializeField] private TMP_Text remainingMoveCount;

        private int _level;
        private int _remainingCount;

        private void OnEnable()
        {
            DataManager.Instance.EventData.OnMatch += RemainingCountHandler;
        }

        private void OnDisable()
        {
            DataManager.Instance.EventData.OnMatch -= RemainingCountHandler;
        }

        private void Start()
        {
            _level = GameManager.Instance.Level - 1;
            _remainingCount = DataManager.Instance.LevelData.LevelRemainingMove[_level];

            remainingMoveCount.text = _remainingCount.ToString();
        }

        private void RemainingCountHandler()
        {
            if (_remainingCount < 0) return;
            
            _remainingCount -= 1;
            remainingMoveCount.text = _remainingCount.ToString();
            DataManager.Instance.EventData.OnCheckRemainingMove?.Invoke(_remainingCount);
        }
    }
}
