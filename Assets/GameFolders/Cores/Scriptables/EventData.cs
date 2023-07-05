using System;
using UnityEngine;

namespace ConnectedFoods.Core
{
    [CreateAssetMenu(fileName = "EventData", menuName = "Data/Event Data")]
    public class EventData : ScriptableObject
    {
        public Action<string> OnSetUsername { get; set; }
        public Action OnLoginSucces { get; set; }
    }
}
