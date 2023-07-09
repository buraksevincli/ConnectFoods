using System.Linq;
using ConnectedFoods.Core;
using ConnectedFoods.Level;
using UnityEngine;

namespace ConnectedFoods.UserInterface
{
    public class GoalPanelController : MonoBehaviour
    {
        [SerializeField] private GoalPanelItem[] goalItems;

        private int _foodScore;
        private int _remainingCount;

        private void Start()
        {
            _remainingCount = DataManager.Instance.LevelData.GetLevelInfo(GameManager.Instance.SelectedLevel).RemainingMove;
            GoalSetter();
        }

        private void OnEnable()
        {
            DataManager.Instance.EventData.OnMatch += GoalUpdateHandler;
        }

        private void OnDisable()
        {
            DataManager.Instance.EventData.OnMatch -= GoalUpdateHandler;
        }
        
        private void GoalSetter()
        {
            RequiredFood[] requiredFoods = DataManager.Instance.LevelData.GetLevelInfo(GameManager.Instance.SelectedLevel).RequiredFoods;

            foreach (GoalPanelItem goalItem in goalItems)
            {
                goalItem.FoodGameObject.SetActive(false);
            }
            
            foreach (RequiredFood requiredFood in requiredFoods)
            {
                GoalPanelItem goalPanelItem = goalItems.FirstOrDefault(goalItem => goalItem.FoodType == requiredFood.FoodType);
                goalPanelItem.FoodGameObject.SetActive(true);
                goalPanelItem.RemainingAmount = requiredFood.Amount;
            }
        }


        private void GoalUpdateHandler(FoodType foodType, int amount)
        {
            _foodScore += amount;

            GoalPanelItem goalPanelItem = goalItems.FirstOrDefault(goalItem => goalItem.FoodType == foodType);
            goalPanelItem.RemainingAmount -= amount;
            
            if (_remainingCount < 0) return;
            
            _remainingCount--;
            
            int totalFoodCount = goalItems.Sum(goalItem => goalItem.RemainingAmount);

            switch (totalFoodCount)
            {
                case 0 when _remainingCount >= 0:
                    int totalScore = _foodScore + _remainingCount;
                    DataManager.Instance.EventData.OnWinCondition?.Invoke(totalScore);
                    break;
                case > 0 when _remainingCount == 0:
                    DataManager.Instance.EventData.OnLoseCondition?.Invoke(_foodScore);
                    break;
            }
        }
    }
}
