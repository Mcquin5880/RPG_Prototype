using System.Collections;
using UnityEngine;
using RPG.Control;
using RPG.Movement;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] Weapon weapon;
        [SerializeField] float respawnTime = 10f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                Pickup(other.GetComponent<Fighter>());
            }
        }

        private void Pickup(Fighter fighter)
        {
            fighter.EquipWeapon(weapon);
            StartCoroutine(HidePickupForSeconds(respawnTime));
        }

        // temp method for testing purposes, might use this for certain items / weapons that respawn
        private IEnumerator HidePickupForSeconds(float timeUntilReactivatePickup)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(timeUntilReactivatePickup);
            ShowPickup(true);
        }

        private void ShowPickup(bool shouldShow)
        {
            GetComponent<Collider>().enabled = shouldShow;
            foreach (Transform child in transform) {
                child.gameObject.SetActive(shouldShow);
            }
        }

        public bool HandleRaycast(Player player)
        {        
            if (Input.GetMouseButtonDown(0))
            {
                Pickup(player.GetComponent<Fighter>());
            }                
            return true;
        }

        public MouseCursorType GetMouseCursorType()
        {
            return MouseCursorType.Pickup;
        }
    }
}
