using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ConnectedFoods.Core;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ConnectedFoods.Game
{
    public class FoodContentController : MonoBehaviour
    {
        [SerializeField] private FoodItem foodItemPrefab;
        [SerializeField, Range(4, 9)] private int size;
        [SerializeField] private Transform startPoint;

        private GridNode[,] _gridNodes;
        private readonly List<FoodItem> _foodItems = new List<FoodItem>();

        private void OnEnable()
        {
            DataManager.Instance.EventData.OnMatch += OnMatchHandler;
        }

        private void OnDisable()
        {
            DataManager.Instance.EventData.OnMatch -= OnMatchHandler;
        }

        private void Start()
        {
            InitiateItems();
        }

        private void InitiateItems()
        {
            _gridNodes = new GridNode[size, size];

            _foodItems.AddRange(GetComponentsInChildren<FoodItem>().ToList());
            
            foreach (FoodItem foodItem in _foodItems)
            {
                foodItem.transform.position = startPoint.position;
                foodItem.SetStartPosition(startPoint.position);
            }
            
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    float x = j - size / 2f + 0.5f;
                    float y = i - size / 2f + 0.5f;
                    
                    _gridNodes[j, i] = new GridNode
                    {
                        Position = new Vector3(x, y, 0),
                        FoodItem = null,
                        IsEmpty = true
                    };
                }
            }
            
            StartCoroutine(FillGrids());
        }

        private void OnMatchHandler()
        {
            StartCoroutine(FillGrids());
        }

        private IEnumerator FillGrids()
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (_gridNodes[j,i].IsEmpty)
                    {
                         bool foundedFood = false;
                        
                        for (int k = i; k < size; k++)
                        {
                            if (_gridNodes[j,k].IsEmpty) continue;
                        
                            foundedFood = true;
                            
                            FoodItem foodItem = _gridNodes[j,k].FoodItem;
                        
                            _gridNodes[j, i].IsEmpty = false;
                            _gridNodes[j, i].FoodItem = foodItem;
                            _gridNodes[j,k].IsEmpty = true;
                        
                            foodItem.GridNode = _gridNodes[j, i];
                            foodItem.MoveToGrid();
                            break;
                        }
                        
                        if (!foundedFood)
                        {
                            FoodItem foodItem = _foodItems.FirstOrDefault(foodItem => !foodItem.IsUsing);
                            foodItem.FoodType = (FoodType)Random.Range(1, Enum.GetValues(typeof(FoodType)).Length);
                            foodItem.IsUsing = true;
                            _gridNodes[j,i].IsEmpty = false;
                            _gridNodes[j,i].FoodItem = foodItem;
                            foodItem.GridNode = _gridNodes[j, i];
                            foodItem.MoveToGrid();
                        }
                    }
                    yield return new WaitForSeconds(0.01f);
                }
                yield return new WaitForSeconds(0.01f);
            }
        }

#if UNITY_EDITOR
        [Button]
        private void Create()
        {
            Clear();

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    FoodItem foodItem = PrefabUtility.InstantiatePrefab(foodItemPrefab, transform) as FoodItem;
                    
                    float scale = foodItem.transform.localScale.x;
                    float x = j - size / 2f + scale / 2f;
                    float y = i - size / 2f + scale / 2f;

                    foodItem.transform.localPosition = new Vector3(x, y, 0);
                    foodItem.FoodType = (FoodType)Random.Range(1, Enum.GetValues(typeof(FoodType)).Length);
                }
            }
        }

        [Button]
        private void Clear()
        {
            FoodItem[] items = GetComponentsInChildren<FoodItem>();

            foreach (FoodItem foodItem in items)
            {
                DestroyImmediate(foodItem.gameObject);
            }
        }
#endif
        
    }
}
