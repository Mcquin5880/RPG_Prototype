using UnityEngine;
using RPG.Saving;
using System;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float experiencePts = 0;

        public event Action onExperienceGained;

        public void GainEXP(float experience)
        {
            experiencePts += experience;
            onExperienceGained();
        }

        public float GetExperiencePts()
        {
            return experiencePts;
        }

        public object CaptureState()
        {
            return experiencePts;
        }

        public void RestoreState(object state)
        {
            experiencePts = (float)state;
        }
    }
}
