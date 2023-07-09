using System.Collections.Generic;
using ConnectedFoods.Core;
using UnityEngine;

namespace ConnectedFoods.Effect
{
    public class BrokenEffector : MonoSingleton<BrokenEffector>
    {
        [SerializeField] private BrokenEffect brokenEffectPrefab;

        private readonly Queue<BrokenEffect> _effects = new Queue<BrokenEffect>();

        public void Broke(Vector3 position)
        {
            if (_effects.Count == 0)
            {
                BrokenEffect newEffect = Instantiate(brokenEffectPrefab, transform);
                _effects.Enqueue(newEffect);
            }

            BrokenEffect effect = _effects.Dequeue();
            effect.Broken(position, ()=> _effects.Enqueue(effect));
        }
    }
}
