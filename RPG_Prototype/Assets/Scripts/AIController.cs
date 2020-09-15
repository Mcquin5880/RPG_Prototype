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
        [SerializeField] float suspicionTime = 3f;
        [SerializeField] WaypointPatrol waypointPatrol;
        [SerializeField] float waypointTolerance = 1.5f;

        Health health;      
        Fighter fighter;
        Mover mover;

        GameObject player;
        float timeSinceSeenPlayer = Mathf.Infinity;
        Vector3 defaultPosition;
        int currentWaypointIndex = 0;


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
                timeSinceSeenPlayer = 0;
                fighter.Attack(player);
            }
            else if (timeSinceSeenPlayer <= suspicionTime)
            {
                GetComponent<ActionScheduler>().CancelCurrentAction();
            }
            else
            {
                //mover.StartMoveAction(defaultPosition);   
                Patrol();
            }
            timeSinceSeenPlayer += Time.deltaTime;
        }

        private bool InAttackRange()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            return distanceToPlayer <= chaseRadius;
        }

        private void Patrol()
        {
            if (waypointPatrol != null)
            {
                float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
                if (distanceToWaypoint <= waypointTolerance)
                {

                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseRadius);
        }
    }
}
