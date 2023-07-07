using System;
using System.Collections.Generic;
using ConnectedFoods.Core;
using UnityEngine;

namespace ConnectedFoods.Game
{
    public class MatchController : MonoSingleton<MatchController>
    {
        [SerializeField] private int minMatchCount;

        [NonSerialized] public FoodType CurrentFoodType;
        [NonSerialized] public int ListCount;

        private List<FoodItem> _selectedItems = new List<FoodItem>();

        private LineController _lineController;
        private OverlapCircleController _overlapCircleController;

        private Vector3 _mousePosition;
        private Camera _camera;

        protected override void Awake()
        {
            base.Awake();
            _camera = Camera.main;

            _lineController = new LineController(this);
            _overlapCircleController = new OverlapCircleController();
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                _mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);

                _lineController.LineRenderer(_selectedItems, _mousePosition, _camera);
                _overlapCircleController.OverlapCircle(_selectedItems, _mousePosition, _camera);
                
                if (_selectedItems.Count > 1)
                {
                    float mouseToLast = Vector3.Distance(_selectedItems[^1].transform.position, _mousePosition);
                    float mouseToBeforeLast = Vector3.Distance(_selectedItems[^2].transform.position, _mousePosition);

                    if (mouseToLast > mouseToBeforeLast + .03f)
                    {
                        _selectedItems[^1]._isSelected = false;
                        _selectedItems[^1].transform.localScale = Vector3.one;
                        _selectedItems.RemoveAt(_selectedItems.Count - 1);
                    }
                }
            }
        }

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
                CurrentFoodType = foodItem.FoodType;
                _selectedItems.Add(foodItem);
                foodItem.transform.localScale = Vector3.one * 0.8f;
            }
            else
            {
                float distance = Vector3.Distance(_selectedItems[^1].transform.position,
                    foodItem.transform.position);

                if (CurrentFoodType == foodItem.FoodType && distance < 1.5f) // Matching is proceeding successfully
                {
                    foodItem._isSelected = true;
                    _selectedItems.Add(foodItem);
                    foodItem.transform.localScale = Vector3.one * 0.8f;
                }
            }
        }

        public void CheckMatch()
        {
            if (_selectedItems.Count < minMatchCount)
            {
                foreach (FoodItem selectedItem in _selectedItems)
                {
                    selectedItem.SelectionReset();
                }
            }
            else
            {
                foreach (FoodItem selectedItem in _selectedItems)
                {
                    selectedItem.OnMatch();
                }

                ListCount = _selectedItems.Count;
                DataManager.Instance.EventData.OnMatch?.Invoke();
            }

            CurrentFoodType = FoodType.None;
            _selectedItems.Clear();
            _lineController.LineDisable();
        }
        
        // private void OnDrawGizmos()
        // {
        //     OnDrawGizmosSelected();
        // }
        //
        // private void OnDrawGizmosSelected()
        // {
        //     _overlapCircleController.OnDrawGizmosSelected();
        // }


        // private int maxLevel;
        //
        // public int Level
        // {
        //     
        //     get => PlayerPrefs.GetInt("Level", 1);
        //     set
        //     {
        //         if (value <= maxLevel)
        //         {
        //             PlayerPrefs.SetInt("Level", value);
        //         }
        //     }
        // }
    }
}