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

        private void Start()
        {
            GoalSetter();
        }

        private void OnEnable()
        {
            DataManager.Instance.EventData.OnMatch += GoalUpdateHandler;
            DataManager.Instance.EventData.OnCheckRemainingMove += OnCheckRemainingMoveHandler;
        }

        private void OnDisable()
        {
            DataManager.Instance.EventData.OnMatch -= GoalUpdateHandler;
            DataManager.Instance.EventData.OnCheckRemainingMove -= OnCheckRemainingMoveHandler;
        }
        
        private void OnCheckRemainingMoveHandler(int remainingMove)
        {
            int totalFoodCount = goalItems.Sum(goalItem => goalItem.RemainingAmount);

            switch (totalFoodCount)
            {
                case 0 when remainingMove >= 0:
                    int totalScore = _foodScore + remainingMove;
                    DataManager.Instance.EventData.OnWinCondition?.Invoke(totalScore);
                    break;
                case > 0 when remainingMove == 0:
                    DataManager.Instance.EventData.OnLoseCondition?.Invoke(_foodScore);
                    break;
            }
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
        }
    }
}
