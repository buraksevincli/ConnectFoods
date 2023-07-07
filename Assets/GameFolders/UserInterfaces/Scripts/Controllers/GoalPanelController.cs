using ConnectedFoods.Core;
using ConnectedFoods.Game;
using TMPro;
using UnityEngine;

namespace ConnectedFoods.UserInterface
{
    public class GoalPanelController : MonoBehaviour
    {
        [SerializeField] private GameObject apple;
        [SerializeField] private TMP_Text appleGoal;
        
        [SerializeField] private GameObject banana;
        [SerializeField] private TMP_Text bananaGoal;

        [SerializeField] private GameObject blop;
        [SerializeField] private TMP_Text blopGoal;

        [SerializeField] private GameObject blueberries;
        [SerializeField] private TMP_Text blueberriesGoal;

        [SerializeField] private GameObject dragonFruit;
        [SerializeField] private TMP_Text dragonFruitGoal;

        private int _foodScore;
        private int _totalScore;
        private int _remainingMove;
        private int _totalFoodCount;
        
        private int _level;
        private int _appleGoalCount;
        private int _bananaGoalCount;
        private int _blopGoalCount;
        private int _blueberriesGoalCount;
        private int _dragonFruitGoalCount;
        
        private void Start()
        {
            _level = GameManager.Instance.Level - 1;

            _remainingMove = DataManager.Instance.LevelData.LevelRemainingMove[_level];
            _appleGoalCount = DataManager.Instance.LevelData.LevelRequiredApple[_level];
            _bananaGoalCount = DataManager.Instance.LevelData.LevelRequiredBanana[_level];
            _blopGoalCount = DataManager.Instance.LevelData.LevelRequiredBlob[_level];
            _blueberriesGoalCount = DataManager.Instance.LevelData.LevelRequiredBlueberries[_level];
            _dragonFruitGoalCount = DataManager.Instance.LevelData.LevelRequiredDragonFruit[_level];

            GoalSetter(_level);
        }

        private void OnEnable()
        {
            DataManager.Instance.EventData.OnMatch += GoalUpdateHandler;
            DataManager.Instance.EventData.OnCheckRemainingMove += RemainingMoveHandler;
        }

        private void OnDisable()
        {
            DataManager.Instance.EventData.OnMatch -= GoalUpdateHandler;
            DataManager.Instance.EventData.OnCheckRemainingMove -= RemainingMoveHandler;
        }

        private void Update()
        {
            _totalFoodCount = _appleGoalCount + _bananaGoalCount + _blopGoalCount + _blueberriesGoalCount +
                              _dragonFruitGoalCount;

            if (_totalFoodCount == 0 && _remainingMove >= 0)
            {
                _totalScore = _foodScore + _remainingMove;
                DataManager.Instance.EventData.OnWinCondition?.Invoke(_totalScore);
            }
            else if(_totalFoodCount > 0 && _remainingMove == 0)
            {
                DataManager.Instance.EventData.OnLoseCondition?.Invoke(_foodScore);
            }
            
        }

        private void RemainingMoveHandler(int score)
        {
            _remainingMove = score;
        }

        private void GoalSetter(int level)
        {
            if (_appleGoalCount != 0)
            {
                apple.SetActive(true);
                appleGoal.text = _appleGoalCount.ToString();
            }
            else
            {
                apple.SetActive(false);
            }

            if (_bananaGoalCount != 0)
            {
                banana.SetActive(true);
                bananaGoal.text = _bananaGoalCount.ToString();
            }
            else
            {
                banana.SetActive(false);
            }
            
            if (_blopGoalCount != 0)
            {
                blop.SetActive(true);
                blopGoal.text = _blopGoalCount.ToString();
            }
            else
            {
                blop.SetActive(false);
            }
            
            if (_blueberriesGoalCount != 0)
            {
                blueberries.SetActive(true);
                blueberriesGoal.text = _blueberriesGoalCount.ToString();
            }
            else
            {
                blueberries.SetActive(false);
            }
            
            if (_dragonFruitGoalCount != 0)
            {
                dragonFruit.SetActive(true);
                dragonFruitGoal.text = _dragonFruitGoalCount.ToString();
            }
            else
            {
                dragonFruit.SetActive(false);
            }
        }

        private void GoalUpdateHandler()
        {
            switch (MatchController.Instance.CurrentFoodType)
            {
                case FoodType.Apple:
                    if (_appleGoalCount > 0)
                    {
                        _appleGoalCount -= MatchController.Instance.ListCount;
                        _foodScore += MatchController.Instance.ListCount;
                    }
                    if (_appleGoalCount < 0)
                    {
                        _appleGoalCount = 0;
                    }

                    appleGoal.text = _appleGoalCount.ToString();
                    break;
                case  FoodType.Banana:
                    if (_bananaGoalCount > 0)
                    {
                        _bananaGoalCount -= MatchController.Instance.ListCount;
                        _foodScore += MatchController.Instance.ListCount;
                    }

                    if (_bananaGoalCount < 0)
                    {
                        _bananaGoalCount = 0;
                    }
                    
                    bananaGoal.text = _bananaGoalCount.ToString();
                    break;
                case FoodType.Blob:
                    if (_blopGoalCount > 0)
                    {
                        _blopGoalCount -= MatchController.Instance.ListCount;
                        _foodScore += MatchController.Instance.ListCount;
                    }

                    if (_blopGoalCount < 0)
                    {
                        _blopGoalCount = 0;
                    }
                    
                    blopGoal.text = _blopGoalCount.ToString();
                    break;
                case FoodType.Blueberries:
                    if (_blueberriesGoalCount > 0)
                    {
                        _blueberriesGoalCount -= MatchController.Instance.ListCount;
                        _foodScore += MatchController.Instance.ListCount;
                    }

                    if (_blueberriesGoalCount < 0)
                    {
                        _blueberriesGoalCount = 0;
                    }
                    
                    blueberriesGoal.text = _blueberriesGoalCount.ToString();
                    break;
                case FoodType.DragonFruit:
                    if (_dragonFruitGoalCount > 0)
                    {
                        _dragonFruitGoalCount -= MatchController.Instance.ListCount;
                        _foodScore += MatchController.Instance.ListCount;
                    }

                    if (_dragonFruitGoalCount < 0)
                    {
                        _dragonFruitGoalCount = 0;
                    }
                    
                    dragonFruitGoal.text = _dragonFruitGoalCount.ToString();
                    break;
            }
        }
    }
}
