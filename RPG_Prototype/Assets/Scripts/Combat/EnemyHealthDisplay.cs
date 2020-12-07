using RPG.Attributes;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Fighter playerFighterComponent;
        Health target;

        private void Awake()
        {
            playerFighterComponent = GameObject.FindGameObjectWithTag("Player").GetComponent<Fighter>();
        }

        private void Update()
        {
            target = playerFighterComponent.GetTarget();
            if (target != null)
            {
                GetComponent<Text>().text = String.Format("{0:0}/{1:0}", target.GetHealthPoints(), target.GetMaxHealthPoints());
            }
            else
            {
                GetComponent<Text>().text = "N/A";
            }
        }
    }
}
