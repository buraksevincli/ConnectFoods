using UnityEngine;

namespace ConnectedFoods.Core
{
    public class DataManager : MonoSingleton<DataManager>
    {
        [SerializeField] private EventData eventData;

        public EventData EventData => eventData;
    }
}
