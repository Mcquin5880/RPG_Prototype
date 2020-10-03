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

        Health target;
        float timeSincePreviousAttack = Mathf.Infinity;

        private void Update()
        {

            timeSincePreviousAttack += Time.deltaTime;
            
            if (target == null || !target.IsAlive()) return;

            if (target != null && !InRangeOfTarget())
            {
                GetComponent<Mover>().MoveToLocation(target.transform.position, 1f);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                transform.LookAt(target.transform);
                if (timeSincePreviousAttack >= timeBetweenAttacks)
                {
                    GetComponent<Animator>().ResetTrigger("stopAttacking");
                    GetComponent<Animator>().SetTrigger("attack");
                    timeSincePreviousAttack = 0;
                }
            }           
        }   

        // Animation event
        void Hit()
        {
            if (target == null) return;
            target.TakeDamage(weaponDamage);
        }

        private bool InRangeOfTarget()
        {
            return Vector3.Distance(transform.position, target.transform.position) < attackRange;
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;

            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && targetToTest.IsAlive();
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();   
        }

        public void Cancel()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttacking");
            target = null;
            GetComponent<Mover>().Cancel();
        }
     
    }
}
