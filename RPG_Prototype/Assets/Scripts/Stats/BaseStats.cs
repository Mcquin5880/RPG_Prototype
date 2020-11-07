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
        [SerializeField] bool shouldUseModifiers = false;

        public event Action onLevelUp;

        int currentLevel = 0;
        Experience experience;

        private void Awake()
        {
            experience = GetComponent<Experience>();
        }

        private void Start()
        {
            currentLevel = CalculateLevel();         
        }

        private void OnEnable()
        {
            if (experience != null)
            {
                experience.onExperienceGained += UpdateLevel;
            }
        }

        private void OnDisable()
        {
            if (experience != null)
            {
                experience.onExperienceGained -= UpdateLevel;
            }
        }

        public float GetStat(Stat stat)
        {
            return (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1 + GetPercentageModifier(stat) / 100);
        }

        private float GetBaseStat(Stat stat)
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

        private int CalculateLevel()
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

        private float GetAdditiveModifier(Stat stat)
        {
            if (!shouldUseModifiers) return 0;

            float sum = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetAdditiveModifiers(stat))
                {
                    sum += modifier;
                }
            }
            return sum;
        }

        private float GetPercentageModifier(Stat stat)
        {
            if (!shouldUseModifiers) return 0;

            float sum = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetPercentageModifiers(stat))
                {
                    sum += modifier;
                }
            }
            return sum;
        }
    }
}
