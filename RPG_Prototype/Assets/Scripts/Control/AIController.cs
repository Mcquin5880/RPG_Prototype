using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;
using RPG.Attributes;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] WaypointPatrol waypointPatrol;
        [SerializeField] float chaseRadius = 7f;      
        [SerializeField] float suspicionTime = 3f;
        [SerializeField] float waypointPauseTime = 4f;
        [SerializeField] float waypointTolerance = 1.5f;
        [Range(0, 1)] [SerializeField] float patrolSpeedFraction = 0.2f;

        Health health;      
        Fighter fighter;
        MovementHandler mover;

        GameObject player;
        float timeSinceSeenPlayer = Mathf.Infinity;
        float timeSinceWaypointArrival = Mathf.Infinity;
        Vector3 defaultPosition;
        int currentWaypointIndex = 0;

        private void Awake()
        {
            health = GetComponent<Health>();
            fighter = GetComponent<Fighter>();
            mover = GetComponent<MovementHandler>();
            player = GameObject.FindWithTag("Player");
        }

        private void Start()
        {         
            defaultPosition = transform.position;
        }

        private void Update()
        {
            if (health.IsDead()) return;

            if (InAttackRange() && fighter.CanAttack(player))
            {
                Attack();
            }
            else if (timeSinceSeenPlayer <= suspicionTime)
            {
                GetComponent<ActionScheduler>().CancelCurrentAction();
            }
            else
            {                
                Patrol();
            }
            timeSinceSeenPlayer += Time.deltaTime;
            timeSinceWaypointArrival += Time.deltaTime;
        }

        private void Attack()
        {
            Debug.Log(this.name + " is chasing player!");
            timeSinceSeenPlayer = 0;
            fighter.Attack(player);
        }

        private bool InAttackRange()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            return distanceToPlayer <= chaseRadius;
        }

        private void Patrol()
        {
            Vector3 nextPosition = defaultPosition;

            if (waypointPatrol != null)
            {
                if (AtWaypoint())
                {
                    timeSinceWaypointArrival = 0;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }

            if (timeSinceWaypointArrival > waypointPauseTime)
            {        
                mover.StartMoveAction(nextPosition, patrolSpeedFraction);
            }        
        }

        private bool AtWaypoint()
        {
            float distanceToNextWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToNextWaypoint <= waypointTolerance;
        }

        private Vector3 GetCurrentWaypoint()
        {
            return waypointPatrol.GetWaypoint(currentWaypointIndex);
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = waypointPatrol.GetNextIndex(currentWaypointIndex);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseRadius);
        }
    }
}
