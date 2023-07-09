using System;
using ConnectedFoods.Core;
using UnityEngine;

namespace ConnectedFoods.Game
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        private Camera _camera;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
        }

        private void Start()
        {
            _camera.orthographicSize = DataManager.Instance.LevelData.GetLevelInfo(GameManager.Instance.SelectedLevel).Size + 1;
        }
    }
}
