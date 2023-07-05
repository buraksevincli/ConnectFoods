using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ConnectedFoods.Core;
using DG.Tweening;
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
            }
            
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    float x = j - size / 2f + 0.5f;
                    float y = i - size / 2f + 0.5f;
                    
                    _gridNodes[i, j] = new GridNode
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
                    if (_gridNodes[i,j].IsEmpty)
                    {
                        bool foundedFood = false;
                        
                        for (int k = j; k < size; k++)
                        {
                            if (_gridNodes[i,k].IsEmpty) continue;

                            foundedFood = true;
                            
                            FoodItem foodItem = _gridNodes[i,k].FoodItem;

                            _gridNodes[i,k].IsEmpty = false;
                            foodItem.GridNode = _gridNodes[i, k];
                            foodItem.MoveToGrid();
                        }
                        
                        if (!foundedFood)
                        {
                            FoodItem foodItem = _foodItems.FirstOrDefault(foodItem => !foodItem.IsUsing);
                            foodItem.IsUsing = true;
                            _gridNodes[i,j].IsEmpty = false;
                            _gridNodes[i,j].FoodItem = foodItem;
                            foodItem.GridNode = _gridNodes[i, j];
                            foodItem.MoveToGrid();
                        }
                    }
                    yield return new WaitForSeconds(0.05f);
                }
                yield return new WaitForSeconds(0.1f);
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
