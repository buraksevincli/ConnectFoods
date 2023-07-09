using System;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

namespace ConnectedFoods.Effect
{
    public class BrokenEffect : MonoBehaviour
    {
        [SerializeField] private GameObject brokenObject;
        
        public async void Broken(Vector3 position, Action onComplete = null)
        {
            brokenObject.SetActive(true);
            brokenObject.transform.position = position;

            await Task.Delay(500);

            brokenObject.SetActive(false);
            onComplete?.Invoke();
        }
    }
}
