using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseRadius = 7f;

        Health health;      
        Fighter fighter;
        Mover mover;
        GameObject player;
        Vector3 defaultPosition;

        private void Start()
        {
            health = GetComponent<Health>();
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
            player = GameObject.FindWithTag("Player");
            defaultPosition = transform.position;
        }

        private void Update()
        {
            if (!health.IsAlive()) return;

            if (InAttackRange() && fighter.CanAttack(player))
            {
                Debug.Log(this.name + " chasing player!");
                fighter.Attack(player);
            }
            else
            {
                mover.StartMoveAction(defaultPosition);               
            }
        }

        private bool InAttackRange()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            return distanceToPlayer <= chaseRadius;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseRadius);
        }
    }
}
