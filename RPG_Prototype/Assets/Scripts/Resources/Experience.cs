using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Resources
{
    public class Experience : MonoBehaviour
    {
        [SerializeField] float experiencePts = 0;

        public void GainEXP(float experience)
        {
            experiencePts += experience;
        }
    }
}
