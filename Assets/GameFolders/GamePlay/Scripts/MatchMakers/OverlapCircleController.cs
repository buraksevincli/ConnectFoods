using System.Collections.Generic;
using ConnectedFoods.Core;
using UnityEngine;

namespace ConnectedFoods.Game
{
    public class OverlapCircleController
    {
        private Vector2 circleCenter;
        private Collider2D _collider;
        
        private float _radius = .3f;


        public void OverlapCircle(Vector3 center, float radius)
        {
            _radius = radius;
            circleCenter = center;

            _collider = Physics2D.OverlapCircle(circleCenter, radius);

            if (_collider)
            {
                _collider.TryGetComponent(out FoodItem foodItem);
                if (MatchController.Instance.currentFoodType != foodItem.FoodType || foodItem._isSelected) return;
                
                DataManager.Instance.EventData.OnSelectFoodItem?.Invoke(foodItem);
            }
        }

        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(circleCenter, _radius);
        }
    }
}
