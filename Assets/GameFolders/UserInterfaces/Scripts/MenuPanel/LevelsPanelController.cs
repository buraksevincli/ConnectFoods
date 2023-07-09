using System;
using System.Collections;
using System.Collections.Generic;
using ConnectedFoods.Core;
using ConnectedFoods.Level;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace ConnectedFoods.UserInterface
{
    public class LevelsPanelController : MonoBehaviour
    {
        [SerializeField] private LevelButtonItem levelButtonItemPrefab;
        [SerializeField] private RectTransform contentRectTransform;
        [SerializeField] private ScrollRect scrollRect;

        private Action _newLevelAction;
        private Action _openButtonAction;

        private IEnumerator Start()
        {
            if (GameManager.Instance.LastGameState == GameState.HighScoreWin)
            {
                yield return new WaitForSeconds(1f);
            }
            
            int maxLevel = DataManager.Instance.LevelData.MaxLevel;

            for (int i = 0; i < maxLevel; i++)
            {
                LevelButtonItem levelButtonItem = Instantiate(levelButtonItemPrefab, contentRectTransform);
                LevelStatus levelStatus;
                
                if ((i + 1) < GameManager.Instance.Level)
                {
                    levelStatus = LevelStatus.Active;
                }
                else if ((i + 1) == GameManager.Instance.Level)
                {
                    levelStatus = LevelStatus.Current;
                }
                else
                {
                    levelStatus = LevelStatus.Deactivate;
                }
                
                levelButtonItem.Initiate((i + 1), levelStatus, OnClickLoadLevel, ref _newLevelAction, ref _openButtonAction);
                yield return null;
            }

            _openButtonAction?.Invoke();
            
            StartCoroutine(UpdateScroll());
        }

        private void OnClickLoadLevel(int level)
        {
            GameManager.Instance.LastGameState = GameState.None;
            GameManager.Instance.SelectedLevel = level;
            GameManager.Instance.IsOpenNewLevel = level == GameManager.Instance.Level;
            SceneManager.LoadScene(1);
        }

        private IEnumerator UpdateScroll()
        {
            yield return new WaitForEndOfFrame();

            float offset = (GameManager.Instance.Level -1) * 50;
            float itemOffset = (GameManager.Instance.Level -1) * 250;
            float posY = (50 + itemOffset);
            if (posY + 1000 > contentRectTransform.rect.height)
            {
                posY = contentRectTransform.rect.height - 1000;
            }
            float value = 0;

            float speed = GameManager.Instance.Level * 250;
            
            while (value < posY)
            {
                value += Time.deltaTime * speed;
                contentRectTransform.anchoredPosition = Vector2.up * value;
                yield return null;
            }
            
            _newLevelAction?.Invoke();
            contentRectTransform.anchoredPosition = Vector2.up * posY;
        }
    }
}
