using ConnectedFoods.Data;
using UnityEngine;

namespace ConnectedFoods.Core
{
    public class DataManager : MonoSingleton<DataManager>
    {
        [SerializeField] private EventData eventData;
        [SerializeField] private LevelData levelData;

        public EventData EventData => eventData;
        public LevelData LevelData => levelData;
    }
}
