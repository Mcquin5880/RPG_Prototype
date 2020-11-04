using UnityEngine;
using RPG.Saving;
using RPG.Core;
using RPG.Stats;

namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISaveable
    {
        float health = -1f;
        bool isAlive = true;

        private void Start()
        {
            if (health < 0)
            {
                health = GetComponent<BaseStats>().GetStat(Stat.Health);
            }
        }

        public bool IsAlive()
        {
            return isAlive;
        }

        public void TakeDamage(GameObject damageDealer, float damage)
        {
            // Mathf.Max function to keep health reaching negative values
            health = Mathf.Max(health - damage, 0);
            if (health == 0 && isAlive)
            {
                GetComponent<Animator>().SetTrigger("die");
                isAlive = false;
                GetComponent<ActionScheduler>().CancelCurrentAction();
                GetComponent<CapsuleCollider>().enabled = false;
                GiveExperience(damageDealer);
            }
            Debug.Log("Current health: " + health);
        }

        private void GiveExperience(GameObject damageDealer)
        {
            Experience experience = damageDealer.GetComponent<Experience>();
            if (experience != null)
            {
                experience.GainEXP(GetComponent<BaseStats>().GetStat(Stat.Experience));
            }
        }

        public float GetHealthAsPercentage()
        {
            return 100 * health / GetComponent<BaseStats>().GetStat(Stat.Health);
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
