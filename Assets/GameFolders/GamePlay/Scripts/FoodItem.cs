using System;
using ConnectedFoods.Core;
using ConnectedFoods.Data;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ConnectedFoods.Game
{
    public class FoodItem : MonoBehaviour
    {
        [SerializeField] private FoodType foodType;
        [SerializeField] private FoodData foodData;
        [SerializeField] private SpriteRenderer foodSpriteRenderer;
        [SerializeField] private GameObject visualObject;

        private Transform _transform;
        private bool _isSelected;

        public FoodType FoodType
        {
            get => foodType;
            set
            {
                foodType = value;
                foodSpriteRenderer.sprite = foodData.GetFoodSprite(foodType);
            }
        }

        public GridNode GridNode { get; set; }

        public bool IsUsing { get; set; }

        private void Awake()
        {
            _transform = GetComponent<Transform>(); // For performance
        }

        private void Start()
        {
            visualObject.SetActive(false);
        }

        private void OnValidate()
        {
            if (foodData != null && foodSpriteRenderer != null)
            {
                foodSpriteRenderer.sprite = foodData.GetFoodSprite(foodType);
            }
        }

        public void MoveToGrid()
        {
            visualObject.SetActive(true);
            Vector3 localPosition = _transform.localPosition;
            _transform.localPosition = new Vector3(GridNode.Position.x, localPosition.y, localPosition.z);
            transform.DOLocalMoveY(GridNode.Position.y, 1f);
        }

        public void OnMatch()
        {
            transform.localScale = Vector3.one;
            IsUsing = false;
            visualObject.SetActive(false);
            GridNode.IsEmpty = true;
            GridNode.FoodItem = null;
        }
        
        private void OnMouseEnter()
        {
            if (!MatchController.Instance.IsChoosing || _isSelected) return;

            _isSelected = true;

            DataManager.Instance.EventData.OnSelectFoodItem?.Invoke(this);
            transform.localScale = Vector3.one * 0.8f;
        }

        private void OnMouseDown()
        {
            _isSelected = true;
            MatchController.Instance.IsChoosing = true;
            DataManager.Instance.EventData.OnSelectFoodItem?.Invoke(this);
            transform.localScale = Vector3.one * 0.8f;
        }

        private void OnMouseUp()
        {
            MatchController.Instance.CheckMatch();
        }

        public void SelectionReset()
        {
            _isSelected = false;
            transform.localScale = Vector3.one;
        }
    }
}