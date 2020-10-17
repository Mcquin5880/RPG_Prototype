using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;

namespace RPG.Core
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float health = 100f;
        bool isAlive = true;

        public bool IsAlive()
        {
            return isAlive;
        }

        public void TakeDamage(float damage)
        {
            // Mathf.Max function to keep health from going below 0
            health = Mathf.Max(health - damage, 0);
            if (health == 0 && isAlive)
            {
                GetComponent<Animator>().SetTrigger("die");
                isAlive = false;
                GetComponent<ActionScheduler>().CancelCurrentAction();
                GetComponent<CapsuleCollider>().enabled = false;
            }
            Debug.Log("Current health: " + health);
        }

        // -----------------------------------------------------------------------------------------------------------------------
        // Saving
        // -----------------------------------------------------------------------------------------------------------------------

        public object CaptureState()
        {
            return health;
        }

        public void RestoreState(object state)
        {
            health = (float)state;

            if (health == 0)
            {
                if (!isAlive) return;

                GetComponent<Animator>().SetTrigger("die");
                isAlive = false;
                GetComponent<ActionScheduler>().CancelCurrentAction();
            }
        }
    }
}
