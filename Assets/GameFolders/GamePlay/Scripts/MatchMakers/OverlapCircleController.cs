using System.Collections.Generic;
using ConnectedFoods.Core;
using UnityEngine;

namespace ConnectedFoods.Game
{
    public class OverlapCircleController
    {
        private Vector2 origin;
        private Vector2 direction;
        private Vector2 circleCenter;
        private Collider2D _collider;
        
        private float distance = 3f;
        private float radius = .3f;
        

        public void OverlapCircle(List<FoodItem> selectedItems, Vector3 mousePosition, Camera camera)
        {
            mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
            origin = selectedItems[^1].transform.position;
            direction = (selectedItems[^1].transform.position - mousePosition).normalized;
            circleCenter = origin + direction * -distance;

            _collider = Physics2D.OverlapCircle(circleCenter, radius);

            if (_collider)
            {
                _collider.TryGetComponent(out FoodItem foodItem);
                if (MatchController.Instance.CurrentFoodType != foodItem.FoodType || foodItem._isSelected) return;
                
                DataManager.Instance.EventData.OnSelectFoodItem?.Invoke(foodItem);
            }
        }

        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(circleCenter, radius);
        }
    }
}
