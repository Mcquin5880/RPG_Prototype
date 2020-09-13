using RPG.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseRadius = 7f;

        GameObject player;
        Fighter fighter;

        private void Start()
        {
            fighter = GetComponent<Fighter>();
            player = GameObject.FindWithTag("Player");
        }

        private void Update()
        {
            if (InAttackRange() && fighter.CanAttack(player))
            {
                Debug.Log(this.name + " chasing player!");
                fighter.Attack(player);
            }
            else
            {
                fighter.Cancel();
            }
        }

        private bool InAttackRange()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            return distanceToPlayer <= chaseRadius;
        }
    }
}
