using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Health health = null;
        [SerializeField] RectTransform foreground = null;
        [SerializeField] Canvas canvas = null;

        private void Update()
        {
            if (Mathf.Approximately(health.GetFraction(), 0))
            {
                canvas.enabled = false;
                return;
            }

            foreground.localScale = new Vector3(health.GetFraction(), 1, 1);
        }
    }
}

