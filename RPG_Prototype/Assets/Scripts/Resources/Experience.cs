using UnityEngine;
using RPG.Saving;

namespace RPG.Resources
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float experiencePts = 0;     

        public void GainEXP(float experience)
        {
            experiencePts += experience;
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
