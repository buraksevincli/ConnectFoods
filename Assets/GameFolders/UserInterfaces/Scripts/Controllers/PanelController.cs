using UnityEngine;

namespace ConnectedFoods.UserInterface
{
    public class PanelController : MonoBehaviour
    {
        [SerializeField] private GameObject chatManager;

        public void SetChatManager()
        {
            if (chatManager.activeSelf)
            {
                chatManager.SetActive(false);
            }
            else
            {
                chatManager.SetActive(true);
            }
        }
    }
}
