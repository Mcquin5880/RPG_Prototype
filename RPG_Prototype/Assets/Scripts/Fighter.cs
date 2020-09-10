using RPG.Core;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour
    {
        [SerializeField] float attackRange = 2f;

        Transform target;

        private void Update()
        {
            if (target == null) return;

            if (target != null && !InRangeOfTarget())
            {
                GetComponent<Mover>().MoveToLocation(target.position);
            }
            else
            {
                GetComponent<Mover>().Stop();
            }
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
