﻿using UnityEngine;
using RPG.Attributes;
using RPG.Control;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        public bool HandleRaycast(Player player)
        {
            if (!player.GetComponent<Fighter>().CanAttack(gameObject))
            {
                return false;
            }

            if (Input.GetMouseButton(0))
            {
                player.GetComponent<Fighter>().Attack(gameObject);
            }

            return true;
        }

        public MouseCursorType GetMouseCursorType()
        {
            return MouseCursorType.Combat;
        }
    }
}
