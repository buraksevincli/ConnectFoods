using System;
using System.Collections.Generic;
using ConnectedFoods.Core;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ConnectedFoods.Game
{
    public class MatchController : MonoSingleton<MatchController>
    {
        [SerializeField] private int minMatchCount;

        public List<FoodItem> _selectedItems = new List<FoodItem>();

        public bool IsChoosing { get; set; }

        private void OnEnable()
        {
            DataManager.Instance.EventData.OnSelectFoodItem += OnSelectFoodItemHandler;
        }

        private void OnDisable()
        {
            DataManager.Instance.EventData.OnSelectFoodItem -= OnSelectFoodItemHandler;
        }

        private void OnSelectFoodItemHandler(FoodItem foodItem)
        {
            if (_selectedItems.Count == 0)
            {
                _selectedItems.Add(foodItem);
            }
            else
            {
                if (_selectedItems[^1].FoodType == foodItem.FoodType) // Matching is proceeding successfully
                {
                    _selectedItems.Add(foodItem);
                }
                else // Matching is failure
                {
                    IsChoosing = false;
                    Debug.Log("Matching is failure");
                    foodItem.SelectionReset();
                    foreach (FoodItem selectedItem in _selectedItems)
                    {
                        selectedItem.SelectionReset();
                    }

                    _selectedItems.Clear();
                }
            }
        }

        public void CheckMatch()
        {
            IsChoosing = false;
            
            if (_selectedItems.Count < minMatchCount)
            {
                Debug.Log("Not Enough selected food");
                foreach (FoodItem selectedItem in _selectedItems)
                {
                    selectedItem.SelectionReset();
                }

                _selectedItems.Clear();
                return;
            }

            foreach (FoodItem selectedItem in _selectedItems)
            {
                selectedItem.OnMatch();
            }

            _selectedItems.Clear();
            DataManager.Instance.EventData.OnMatch?.Invoke();
        }
    }
}