using RPG.Movement;
using RPG.Saving;
using UnityEngine;
using RPG.Core;
using RPG.Attributes;
using RPG.Stats;
using System.Collections.Generic;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        [SerializeField] float timeBetweenAttacks = 1f;     
        [SerializeField] Transform rightHandSpawnPoint = null;
        [SerializeField] Transform leftHandSpawnPoint = null;
        [SerializeField] Weapon defaultWeapon = null;

        Health target;
        Weapon currentWeapon = null;
        float timeSincePreviousAttack = Mathf.Infinity;

        private void Start()
        {   
            if (currentWeapon == null)
            {
                EquipWeapon(defaultWeapon);
            }
        }

        private void Update()
        {

            timeSincePreviousAttack += Time.deltaTime;
            
            if (target == null || target.IsDead()) return;

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

        public void EquipWeapon(Weapon weapon)
        {
            this.currentWeapon = weapon;
            Animator anim = GetComponent<Animator>();
            weapon.SpawnWeapon(rightHandSpawnPoint, leftHandSpawnPoint, anim);
        }

        public Health GetTarget()
        {
            return target;
        }

        // Animation event methods
        // --------------------------------------------------------------------------------
        void Hit()
        {
            if (target == null) return;

            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);

            if (currentWeapon.HasProjectile())
            {
                currentWeapon.LaunchProjectile(rightHandSpawnPoint, leftHandSpawnPoint, target, gameObject, damage);
            }
            else
            {
                target.TakeDamage(this.gameObject, damage);
            }
        }

        void Shoot()
        {
            Hit();
        }
        // --------------------------------------------------------------------------------

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;

            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
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

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeapon.GetWeaponDamage();
            }
        }
        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return currentWeapon.GetPercentageBonus();
            }
        }


        private bool InRangeOfTarget()
        {
            return Vector3.Distance(transform.position, target.transform.position) < currentWeapon.GetAttackRange();
        }

        public object CaptureState()
        {
            return currentWeapon.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string) state;
            Weapon weapon = UnityEngine.Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }       
    }
}
