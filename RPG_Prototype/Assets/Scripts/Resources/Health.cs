using UnityEngine;
using RPG.Saving;
using RPG.Core;
using RPG.Stats;
using UnityEngine.Events;

namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float regenerationPercentage = 100;
        [SerializeField] UnityEvent takeDamage;

        float health = -1f;
        bool isDead = false;

        private void Start()
        {       
            if (health < 0)
            {
                health = GetComponent<BaseStats>().GetStat(Stat.Health);
            }
        }

        private void OnEnable()
        {
            GetComponent<BaseStats>().onLevelUp += RegenerateHealth;
        }

        private void OnDisable()
        {
            GetComponent<BaseStats>().onLevelUp -= RegenerateHealth;
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(GameObject damageDealer, float damage)
        {       
            // Mathf.Max function to keep health from reaching negative values
            health = Mathf.Max(health - damage, 0);
            if (health == 0 && !IsDead())
            {
                Die(damageDealer);
            }
            else
            {
                takeDamage.Invoke();
            }
        }

        private void Die(GameObject damageDealer)
        {
            GetComponent<Animator>().SetTrigger("die");
            isDead = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
            GetComponent<CapsuleCollider>().enabled = false;
            GiveExperience(damageDealer);
        }

        public float GetHealthPoints()
        {
            return health;
        }

        public float GetMaxHealthPoints()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public float GetHealthAsPercentage()
        {
            return 100 * health / GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void GiveExperience(GameObject damageDealer)
        {
            Experience experience = damageDealer.GetComponent<Experience>();
            if (experience != null)
            {
                experience.GainEXP(GetComponent<BaseStats>().GetStat(Stat.Experience));
            }
        }

        private void RegenerateHealth()
        {
            float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * (regenerationPercentage / 100);
            health = Mathf.Max(health, regenHealthPoints);
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
                if (isDead) return;

                GetComponent<Animator>().SetTrigger("die");
                isDead = true;
                GetComponent<ActionScheduler>().CancelCurrentAction();
            }
        }
    }
}
