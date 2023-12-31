using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ConnectedFoods.Core;
using ConnectedFoods.Game;
using UnityEngine;

namespace ConnectedFoods.Level
{
    public class LevelManager : MonoSingleton<LevelManager>
    {
        [SerializeField] private FoodItem foodItemPrefab;

        private bool _haveMatch;
        
        private bool _isLevelFinish = false;

        private LevelInfo _levelInfo;
        private Vector3 _spawnPosition;

        private GridNode[,] _gridNodes;
        private readonly List<FoodItem> _foodItems = new List<FoodItem>();

        private void Start()
        {
            _levelInfo = DataManager.Instance.LevelData.GetLevelInfo(GameManager.Instance.SelectedLevel);
            InitiateItems();
        }

        private void OnEnable()
        {
            DataManager.Instance.EventData.OnMatch += OnMatchHandler;
            DataManager.Instance.EventData.OnWinCondition += OnWinHandler;
            DataManager.Instance.EventData.OnLoseCondition += OnLoseHandler;
        }

        private void OnDisable()
        {
            DataManager.Instance.EventData.OnMatch -= OnMatchHandler;
            DataManager.Instance.EventData.OnWinCondition -= OnWinHandler;
            DataManager.Instance.EventData.OnLoseCondition -= OnLoseHandler;
        }

        private void InitiateItems()
        {
            _gridNodes = new GridNode[_levelInfo.Size, _levelInfo.Size];


            for (int i = 0; i < (int)Mathf.Pow(_levelInfo.Size, 2); i++)
            {
                _spawnPosition = new Vector3(i % _levelInfo.Size,
                    transform.position.y + (_levelInfo.Size * 0.5f + 0.5f), transform.position.z);

                FoodItem foodItem = Instantiate(foodItemPrefab, transform);
                foodItem.transform.position = _spawnPosition;
                foodItem.SetStartPosition(_spawnPosition);
                _foodItems.Add(foodItem);
                foodItem.FoodType = (FoodType)UnityEngine.Random.Range(1, Enum.GetValues(typeof(FoodType)).Length);
            }

            for (int i = 0; i < _levelInfo.Size; i++)
            {
                for (int j = 0; j < _levelInfo.Size; j++)
                {
                    float x = j - _levelInfo.Size / 2f + 0.5f;
                    float y = i - _levelInfo.Size / 2f + 0.5f;

                    _gridNodes[j, i] = new GridNode
                    {
                        Neighbors = GetNeighbors(j, i),
                        Position = new Vector3(x, y, 0),
                        FoodItem = null,
                        IsEmpty = true
                    };
                }
            }

            StartCoroutine(FillGrids());
        }

        private void OnMatchHandler(FoodType foodType, int amount)
        {
            StartCoroutine(FillGrids());
        }
        
        private void OnWinHandler(int value)
        {
            _isLevelFinish = true;
        }

        private void OnLoseHandler(int value)
        {
            _isLevelFinish = true;
        }

        private IEnumerator FillGrids()
        {
            for (int i = 0; i < _levelInfo.Size; i++)
            {
                bool foundedEmptyGrid = false;
                for (int j = 0; j < _levelInfo.Size; j++)
                {
                    foundedEmptyGrid = false;
                    if (_gridNodes[j, i].IsEmpty)
                    {
                        bool foundedFood = false;

                        for (int k = i; k < _levelInfo.Size; k++)
                        {
                            if (_gridNodes[j, k].IsEmpty) continue;

                            foundedFood = true;

                            FoodItem foodItem = _gridNodes[j, k].FoodItem;

                            _gridNodes[j, i].IsEmpty = false;
                            _gridNodes[j, i].FoodItem = foodItem;
                            _gridNodes[j, k].IsEmpty = true;

                            foodItem.GridNode = _gridNodes[j, i];
                            foodItem.MoveToGrid();
                            foundedEmptyGrid = true;
                            break;
                        }

                        if (!foundedFood)
                        {
                            FoodItem foodItem = _foodItems.FirstOrDefault(foodItem => !foodItem.IsUsing);
                            foodItem.FoodType = (FoodType)UnityEngine.Random.Range(1, Enum.GetValues(typeof(FoodType)).Length);
                            foodItem.IsUsing = true;
                            _gridNodes[j, i].IsEmpty = false;
                            _gridNodes[j, i].FoodItem = foodItem;
                            foodItem.GridNode = _gridNodes[j, i];
                            foodItem.MoveToGrid();
                            foundedEmptyGrid = true;
                            yield return new WaitForSeconds(.01f);
                        }
                    }

                    if (foundedEmptyGrid)
                    {
                        yield return new WaitForSeconds(0.01f);
                    }
                    else
                    {
                        yield return null;
                    }
                }

                if (foundedEmptyGrid)
                {
                    yield return new WaitForSeconds(0.01f);
                }
                else
                {
                    yield return null;
                }
            }

            yield return new WaitForSeconds(0.1f);

            if (_isLevelFinish) yield break;
            
            _haveMatch = false;
            
            for (int i = 0; i < _levelInfo.Size; i++)
            {
                for (int j = 0; j < _levelInfo.Size; j++)
                {
                    GridNode node = _gridNodes[j, i];
                    foreach (GridNode nodeNeighbor in node.Neighbors)
                    {
                        if (nodeNeighbor != node)
                        {
                            if (nodeNeighbor.FoodItem.FoodType == node.FoodItem.FoodType) // Match
                            {
                                foreach (GridNode neighbor in nodeNeighbor.Neighbors)
                                {
                                    if (neighbor != node || neighbor != nodeNeighbor)
                                    {
                                        if (neighbor.FoodItem.FoodType == nodeNeighbor.FoodItem.FoodType) // 3 Match
                                        {
                                            _haveMatch = true;
                                            MatchController.Instance.CanSelect = true;
                                            yield break;
                                        }
                                    }
                                    yield return null;
                                }
                            }
                        }
                        yield return null;
                    }
                    yield return null;
                }
                yield return null;
            }

            if (!_haveMatch)
            {
                StartCoroutine(ReOrderFoods());
            }
        }

        private IEnumerator ReOrderFoods()
        {
            foreach (FoodItem foodItem in _foodItems)
            {
                foodItem.OnMatch();
                yield return null;
            }

            for (int i = 0; i < _levelInfo.Size; i++)
            {
                for (int j = 0; j < _levelInfo.Size; j++)
                {
                    _gridNodes[j, i].FoodItem = null;
                    _gridNodes[j, i].IsEmpty = true;
                    yield return null;
                }
                yield return null;
            }
            
            yield return new WaitForSeconds(1f);

            StartCoroutine(FillGrids());
        }

        private List<GridNode> GetNeighbors(int j, int i)
        {
            List<GridNode> neighbors = new List<GridNode>();

            if (GetNode(j - 1, i) != null)
            {
                neighbors.Add(GetNode(j - 1, i));
            }

            if (GetNode(j + 1, i) != null)
            {
                neighbors.Add(GetNode(j + 1, i));
            }

            if (GetNode(j - 1, i - 1) != null)
            {
                neighbors.Add(GetNode(j - 1, i - 1));
            }

            if (GetNode(j - 1, i + 1) != null)
            {
                neighbors.Add(GetNode(j - 1, i + 1));
            }

            if (GetNode(j, i + 1) != null)
            {
                neighbors.Add(GetNode(j, i + 1));
            }

            if (GetNode(j, i - 1) != null)
            {
                neighbors.Add(GetNode(j, i - 1));
            }

            if (GetNode(j + 1, i + 1) != null)
            {
                neighbors.Add(GetNode(j + 1, i + 1));
            }

            if (GetNode(j + 1, i - 1) != null)
            {
                neighbors.Add(GetNode(j + 1, i - 1));
            }

            return neighbors;
        }

        private GridNode GetNode(int j, int i)
        {
            if (i >= 0 && i < _levelInfo.Size && j >= 0 && j < _levelInfo.Size)
            {
                return _gridNodes[j, i];
            }
            else
            {
                return null;
            }
        }
    }
}