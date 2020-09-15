using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control 
{
    public class WaypointPatrol : MonoBehaviour
    {
        const float gizmoRadius = .3f;

        private void OnDrawGizmos()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                int nextIndex = i + 1;
                if (nextIndex >= transform.childCount)
                {
                    nextIndex = 0;
                }

                Gizmos.DrawSphere(transform.GetChild(i).position, gizmoRadius);
                Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(nextIndex).position);
            }
        }

    }

}


