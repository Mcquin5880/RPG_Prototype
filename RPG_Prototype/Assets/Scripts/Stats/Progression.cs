﻿using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses = null;

        public float GetHealth(CharacterClass characterClass, int level)
        {
            foreach (ProgressionCharacterClass charClass in characterClasses)
            {
                if (charClass.characterClass == characterClass)
                {
                    return charClass.health[level - 1];
                }
            }
            return 0;
        }

        // --------------------------------------------------------------------------------------------------------------------------------------------------
        // Inner-class
        // --------------------------------------------------------------------------------------------------------------------------------------------------

        [System.Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            public float[] health;
        }

        // --------------------------------------------------------------------------------------------------------------------------------------------------
    }
}



