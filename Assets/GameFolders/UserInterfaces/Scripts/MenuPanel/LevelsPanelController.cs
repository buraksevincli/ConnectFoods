using ConnectedFoods.Core;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ConnectedFoods.UserInterface
{
    public class LevelsPanelController : MonoBehaviour
    {
        [SerializeField] private Button[] levelButtons;
        [SerializeField] private TMP_Text[] scoreTexts;

        private int _level;

        private void Start()
        {
            _level = GameManager.Instance.Level -1;

            for (int i = 0; i <= _level; i++)
            {
                levelButtons[i].interactable = true;
                if (levelButtons[i].interactable)
                {
                    scoreTexts[i].text = $"Score: {DataManager.Instance.LevelData.LevelHighScore[i]}";
                    int levelIndex = i;
                    levelButtons[i].onClick?.AddListener(()=> { OnClickLoadLevel(levelIndex); });
                }
            }
        }

        private void OnClickLoadLevel(int level)
        {
            SceneManager.LoadScene(level+1);
        }
    }
}
