using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] float weaponDamage = 5f;
        [SerializeField] float attackRange = 2f;      
        [SerializeField] GameObject weaponPrefab = null;
        [SerializeField] AnimatorOverrideController animatorOverride = null;

        public float GetWeaponDamage()
        {
            return this.weaponDamage;
        }

        public float GetAttackRange()
        {
            return this.attackRange;
        }

        public void SpawnWeapon(Transform handTransform, Animator animator)
        {
            Instantiate(weaponPrefab, handTransform);
            animator.runtimeAnimatorController = animatorOverride;
        }
       
    }
}
