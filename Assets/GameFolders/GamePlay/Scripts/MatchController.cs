using System;
using System.Collections.Generic;
using ConnectedFoods.Core;
using UnityEngine;

namespace ConnectedFoods.Game
{
    public class MatchController : MonoSingleton<MatchController>
    {
        [SerializeField] private int minMatchCount;

        private List<FoodItem> _selectedItems = new List<FoodItem>();

        public FoodType CurrentFoodType;

        private LineRenderer _lineRenderer;
        private Camera _camera;
        private Vector3 _mousePosition;

        //RaySphere Settings
        [SerializeField] private float distance = 1.5f;
        [SerializeField] private float radius = .4f;
        private Vector2 origin;
        private Vector2 direction;
        private Vector2 circleCenter;
        private Collider2D _collider;

        protected override void Awake()
        {
            base.Awake();
            _camera = Camera.main;
            _lineRenderer = GetComponent<LineRenderer>();
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                LineRendererController();

                RayController();

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

                DataManager.Instance.EventData.OnMatch?.Invoke();
            }

            CurrentFoodType = FoodType.None;
            _selectedItems.Clear();
            _lineRenderer.enabled = false;
        }

        private void LineRendererController()
        {
            _lineRenderer.enabled = true;
            _mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            _lineRenderer.positionCount = _selectedItems.Count + 2;

            _lineRenderer.SetPosition(0, _selectedItems[0].transform.position);

            for (int i = 0; i < _selectedItems.Count; i++)
            {
                _lineRenderer.SetPosition(i + 1, _selectedItems[i].transform.position);
            }

            _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, _mousePosition);

            _lineRenderer.startWidth = .2f;
            _lineRenderer.endWidth = .2f;
        }

        private void RayController()
        {
            origin = _selectedItems[^1].transform.position;
            direction = (_selectedItems[^1].transform.position - _mousePosition).normalized;
            circleCenter = origin + direction * -distance;

            _collider = Physics2D.OverlapCircle(circleCenter, radius);

            if (_collider)
            {
                _collider.TryGetComponent(out FoodItem foodItem);
                if (CurrentFoodType != foodItem.FoodType || foodItem._isSelected) return;
                
                DataManager.Instance.EventData.OnSelectFoodItem?.Invoke(foodItem);
            }
        }

        private void OnDrawGizmos()
        {
            OnDrawGizmosSelected();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(circleCenter, radius);
        }

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