using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatManagerController : MonoBehaviour
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
