using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;
using RPG.Resources;

namespace RPG.Movement
{
    public class MovementHandler : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] float maximumSpeed = 6f;
        NavMeshAgent navMeshAgent;
        Animator animator;
        Health health;

        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            health = GetComponent<Health>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            navMeshAgent.enabled = health.IsAlive();
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);                  
            MoveToLocation(destination, speedFraction);
        }

        public void MoveToLocation(Vector3 destination, float speedFraction)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.speed = maximumSpeed * Mathf.Clamp01(speedFraction);
            navMeshAgent.destination = destination;
        }

        private void MoveToCursor()
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                MoveToLocation(hit.point, 1f);
            }
        }

        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            animator.SetFloat("forwardSpeed", speed);
        }

        // -----------------------------------------------------------------------------------------------------------------------
        // Saving
        // -----------------------------------------------------------------------------------------------------------------------

        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            SerializableVector3 position = (SerializableVector3)state;
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = position.ToVector();
            GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}
