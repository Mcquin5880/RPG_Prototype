using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)] [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass charClass;
        [SerializeField] Progression progression = null;

        private void Update()
        {
            if (gameObject.tag == "Player")
            {
                print(GetLevel());
            }
        }

        public float GetStat(Stat stat)
        {
            return progression.GetStat(stat, charClass, GetLevel());
        }

        public int GetLevel()
        {
            Experience experience = GetComponent<Experience>();

            if (experience == null) return startingLevel;

            float currentEXP = experience.GetExperiencePts();
            int penultimateLevel = progression.GetLevels(Stat.RemainingExpUntilLevelUp, charClass);
            for (int level = 1; level <= penultimateLevel; level++)
            {
                float expToLevelUp = progression.GetStat(Stat.RemainingExpUntilLevelUp, charClass, level);
                if (currentEXP < expToLevelUp)
                {
                    return level;
                }
            }

            return penultimateLevel + 1;
        }
    }
}
