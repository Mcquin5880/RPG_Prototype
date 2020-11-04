using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses = null;

        Dictionary<CharacterClass, Dictionary<Stat, float[]>> progressionDict = null;

        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            BuildProgressionDict();
            return progressionDict[characterClass][stat][level];          
        }

        private void BuildProgressionDict()
        {
            if (progressionDict != null) return;

            progressionDict = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();

            foreach (ProgressionCharacterClass progClass in characterClasses)
            {
                var statLookupTable = new Dictionary<Stat, float[]>();

                foreach (ProgressionStat progStat in progClass.stats)
                {
                    statLookupTable[progStat.stat] = progStat.levels;
                }

                progressionDict[progClass.characterClass] = statLookupTable;
            }
        }

        [System.Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            public ProgressionStat[] stats;          
        }

        [System.Serializable]
        class ProgressionStat
        {
            public Stat stat;
            public float[] levels;
        }
    }
}



