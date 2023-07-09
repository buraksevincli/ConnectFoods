using ConnectedFoods.Core;
using ConnectedFoods.Data;
using ConnectedFoods.Effect;
using DG.Tweening;
using UnityEngine;

namespace ConnectedFoods.Game
{
    public class FoodItem : MonoBehaviour
    {
        [SerializeField] private FoodType foodType;
        [SerializeField] private FoodData foodData;
        [SerializeField] private SpriteRenderer foodSpriteRenderer;
        [SerializeField] private GameObject visualObject;
        [SerializeField] private Transform effectReferenceTransform;
        
        private Transform _transform;
        private Vector3 _startPosition;
        private Tween _moveTween;

        public bool _isSelected;

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

        private void OnValidate()
        {
            if (foodData != null && foodSpriteRenderer != null && foodType != FoodType.None)
            {
                foodSpriteRenderer.sprite = foodData.GetFoodSprite(foodType);
            }
        }

        public void MoveToGrid()
        {
            visualObject.SetActive(true);
            Vector3 localPosition = _transform.localPosition;
            _transform.localPosition = new Vector3(GridNode.Position.x, localPosition.y, localPosition.z);
            _moveTween = _transform.DOLocalMoveY(GridNode.Position.y, 0.5f);
        }

        public void OnMatch()
        {
            if (_moveTween != null)
            {
                if (_moveTween.IsPlaying())
                {
                    _moveTween.Kill();
                }
            }
            BrokenEffector.Instance.Broke(effectReferenceTransform.position);
            _transform.localScale = Vector3.one;
            _transform.localPosition = _startPosition;
            IsUsing = false;
            _isSelected = false;
            GridNode.IsEmpty = true;
            GridNode.FoodItem = null;
            visualObject.SetActive(false);
        }
        
        private void OnMouseEnter()
        {
            if (MatchController.Instance.currentFoodType != this.FoodType || _isSelected) return;
        
            DataManager.Instance.EventData.OnSelectFoodItem?.Invoke(this);
        }

        private void OnMouseDown()
        {
            if (!MatchController.Instance.CanSelect) return;

            _isSelected = true;
            DataManager.Instance.EventData.OnSelectFoodItem?.Invoke(this);
        }

        private void OnMouseUp()
        {
            MatchController.Instance.CheckMatch();
        }

        public void SelectionReset()
        {
            _isSelected = false;
            _transform.localScale = Vector3.one;
        }

        public void SetStartPosition(Vector3 startPosition)
        {
            _startPosition = startPosition;
            visualObject.SetActive(false);
        }
    }
}