using RPG.Resources;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        const string weaponName = "Weapon";

        [SerializeField] float weaponDamage = 5f;
        [SerializeField] float percentDamageBonus = 0;
        [SerializeField] float attackRange = 2f;      
        [SerializeField] GameObject equipPrefab = null;
        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile = null;

        public float GetWeaponDamage()
        {
            return this.weaponDamage;
        }

        public float GetAttackRange()
        {
            return this.attackRange;
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        public float GetPercentageBonus()
        {
            return percentDamageBonus;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject damageDealer, float calculatedDamage)
        {
            Projectile projectileInstance = Instantiate(projectile, GetTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, damageDealer, calculatedDamage);
        }

        private Transform GetTransform(Transform rightHand, Transform leftHand)
        {
            Transform handTransform;
            if (isRightHanded)
            {
                handTransform = rightHand;
            }
            else
            {
                handTransform = leftHand;
            }
            return handTransform;
        }

        public void SpawnWeapon(Transform rightHandTransform, Transform leftHandTransform, Animator animator)
        {
            DestroyPreviousWeapon(rightHandTransform, leftHandTransform);

            if (equipPrefab != null)
            {
                Transform handTransform = GetTransform(rightHandTransform, leftHandTransform);
                GameObject weapon = Instantiate(equipPrefab, handTransform);
                weapon.name = weaponName;
            }
            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
            else
            {
                AnimatorOverrideController overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
                if (overrideController != null)
                {
                    animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
                }
            }
        }

        private void DestroyPreviousWeapon(Transform rightHandTransform, Transform leftHandTransform)
        {
            Transform prevWeapon = rightHandTransform.Find(weaponName);
            if (prevWeapon == null)
            {
                prevWeapon = leftHandTransform.Find(weaponName);
            }
            if (prevWeapon == null) return;

            // rename right before destroying as suggested by tutorial
            prevWeapon.name = "DESTROYING_WEAPON";
            Destroy(prevWeapon.gameObject);
        }
        

    }
}
