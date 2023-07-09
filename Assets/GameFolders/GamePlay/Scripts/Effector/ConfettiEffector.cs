using System;
using ConnectedFoods.Core;
using UnityEngine;

namespace ConnectedFoods.Effect
{
    public class ConfettiEffector : MonoBehaviour
    {
        [SerializeField] private ParticleSystem confettiEffect;

        private void Start()
        {
            if (GameManager.Instance.LastGameState == GameState.HighScoreWin)
            {
                PlayConfetti();
            }
        }

        private void PlayConfetti()
        {
            confettiEffect.Play();
        }
    }
}
