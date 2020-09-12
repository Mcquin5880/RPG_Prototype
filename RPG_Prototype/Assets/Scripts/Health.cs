using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
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
            }
            Debug.Log("Current health: " + health);
        }
    }
}
