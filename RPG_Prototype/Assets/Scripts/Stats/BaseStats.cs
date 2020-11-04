using System;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)] [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass charClass;
        [SerializeField] Progression progression = null;
        [SerializeField] GameObject levelUpParticleEffect = null;

        public event Action onLevelUp;

        int currentLevel = 0;

        private void Start()
        {
            currentLevel = CalculateLevel();
            Experience experience = GetComponent<Experience>();
            if (experience != null)
            {
                experience.onExperienceGained += UpdateLevel;
            }
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel)
            {
                currentLevel = newLevel;
                LevelUpEffects();
                onLevelUp();
            }
        }

        private void LevelUpEffects()
        {
            Instantiate(levelUpParticleEffect, transform);
        }

        public float GetStat(Stat stat)
        {
            return progression.GetStat(stat, charClass, GetLevel());
        }

        public int GetLevel()
        {
            // making sure currentLevel is initialized
            if (currentLevel < 1)
            {
                currentLevel = CalculateLevel();
            }
            return currentLevel;
        }

        public int CalculateLevel()
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
