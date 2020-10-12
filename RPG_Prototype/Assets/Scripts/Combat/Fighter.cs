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
        [SerializeField] float timeBetweenAttacks = 1f;     
        [SerializeField] Transform weaponSpawnPoint = null;
        [SerializeField] Weapon weapon = null;

        Health target;
        float timeSincePreviousAttack = Mathf.Infinity;

        private void Start()
        {
            SpawnWeapon();
        }

        private void Update()
        {

            timeSincePreviousAttack += Time.deltaTime;
            
            if (target == null || !target.IsAlive()) return;

            if (target != null && !InRangeOfTarget())
            {
                GetComponent<MovementHandler>().MoveToLocation(target.transform.position, 1f);
            }
            else
            {
                GetComponent<MovementHandler>().Cancel();
                transform.LookAt(target.transform);
                if (timeSincePreviousAttack >= timeBetweenAttacks)
                {
                    GetComponent<Animator>().ResetTrigger("stopAttacking");
                    GetComponent<Animator>().SetTrigger("attack");
                    timeSincePreviousAttack = 0;
                }
            }           
        }

        private void SpawnWeapon()
        {
            if (weapon == null) return;

            Animator anim = GetComponent<Animator>();
            weapon.SpawnWeapon(weaponSpawnPoint, anim);
        }

        // Animation event
        void Hit()
        {
            if (target == null) return;
            target.TakeDamage(weapon.GetWeaponDamage());
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
            GetComponent<MovementHandler>().Cancel();
        }

        private bool InRangeOfTarget()
        {
            return Vector3.Distance(transform.position, target.transform.position) < weapon.GetAttackRange();
        }

    }
}
