using ConnectedFoods.Core;
using TMPro;
using UnityEngine;

namespace ConnectedFoods.UserInterface
{
    public class MovePanelController : MonoBehaviour
    {
        [SerializeField] private TMP_Text remainingMoveCount;

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
            _remainingCount = DataManager.Instance.LevelData.GetLevelInfo(GameManager.Instance.SelectedLevel).RemainingMove;

            remainingMoveCount.text = _remainingCount.ToString();
        }

        private void RemainingCountHandler(FoodType foodType, int amount)
        {
            if (_remainingCount < 0) return;
            
            _remainingCount -= 1;
            remainingMoveCount.text = _remainingCount.ToString();
        }
    }
}
