using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ConnectedFoods.Core
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public int Level { get; set; }
        public int SelectedLevel { get; set; }

        private WaitForSeconds _loadSceneTime;

        protected override void Awake()
        {
            base.Awake();
            _loadSceneTime = new WaitForSeconds(5f);
        }

        private void Start()
        {
            Level = 1;
        }

        public void LoadMenuScene()
        {
            StartCoroutine(LoadMenuSceneCoroutine());
        }

        private IEnumerator LoadMenuSceneCoroutine()
        {
            yield return _loadSceneTime;
            
            SceneManager.LoadScene(0);
        }
    }
}
