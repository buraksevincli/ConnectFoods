using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ConnectedFoods.Core
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public int Level
        {
            get => PlayerPrefs.GetInt("Level", 1);
            set => PlayerPrefs.SetInt("Level", value);
        }

        public GameState LastGameState { get; set; } = GameState.None;
        
        public int SelectedLevel { get; set; }

        private WaitForSeconds _loadSceneTime;

        protected override void Awake()
        {
            base.Awake();
            _loadSceneTime = new WaitForSeconds(5f);
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
