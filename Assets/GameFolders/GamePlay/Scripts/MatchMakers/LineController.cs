using System.Collections.Generic;
using UnityEngine;

namespace ConnectedFoods.Game
{
    public class LineController
    {
        private LineRenderer _lineRenderer;
        
        public LineController(MatchController matchController)
        {
            _lineRenderer = matchController.GetComponent<LineRenderer>();
        }

        public void LineRenderer(List<FoodItem> selectedItems, Vector3 mousePosition)
        {
            _lineRenderer.enabled = true;
            
            _lineRenderer.positionCount = selectedItems.Count + 2;

            _lineRenderer.SetPosition(0, selectedItems[0].transform.position);

            for (int i = 0; i < selectedItems.Count; i++)
            {
                _lineRenderer.SetPosition(i + 1, selectedItems[i].transform.position);
            }
            
            _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, mousePosition);

            _lineRenderer.startWidth = .2f;
            _lineRenderer.endWidth = .2f;
        }

        public void LineDisable()
        {
            _lineRenderer.enabled = false;
        }
    }
}
