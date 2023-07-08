using ConnectedFoods.Core;
using ConnectedFoods.Level;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ConnectedFoods.UserInterface
{
    public class LevelsPanelController : MonoBehaviour
    {
        [SerializeField] private LevelButtonItem levelButtonItemPrefab;
        [SerializeField] private RectTransform contentRectTransform;

        private void Start()
        {
            int maxLevel = DataManager.Instance.LevelData.MaxLevel;

            for (int i = 0; i <= maxLevel; i++)
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
                
                levelButtonItem.Initiate((i + 1), levelStatus, OnClickLoadLevel);
            }
        }

        private void OnClickLoadLevel(int level)
        {
            GameManager.Instance.SelectedLevel = level;
            SceneManager.LoadScene(1);
        }
    }
}
