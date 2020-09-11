using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float attackRange = 2f;
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] float weaponDamage = 5f;

        Transform target;
        float timeSincePreviousAttack = 0;

        private void Update()
        {

            timeSincePreviousAttack += Time.deltaTime;
            
            if (target == null) return;

            if (target != null && !InRangeOfTarget())
            {
                GetComponent<Mover>().MoveToLocation(target.position);
            }
            else
            {
                GetComponent<Mover>().Cancel();

                if (timeSincePreviousAttack >= timeBetweenAttacks)
                {
                    GetComponent<Animator>().SetTrigger("attack");
                    timeSincePreviousAttack = 0;
                }
            }           
        }

        // Animation event
        void Hit()
        {
            target.GetComponent<Health>().TakeDamage(weaponDamage);
        }

        private bool InRangeOfTarget()
        {
            return Vector3.Distance(transform.position, target.position) < attackRange;
        }

        public void Attack(CombatTarget combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.transform;   
        }

        public void Cancel()
        {
            target = null;
        }
     
    }
}
