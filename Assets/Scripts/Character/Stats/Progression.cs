using Character.Abilities;
using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Character.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Character/Stats/New progression")]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionPerLevel[] progression;

        public float GetStatistic(StatType statType, int level)
        {
            return progression[level - 1].GetStatistic(statType);
        }

        public Ability[] GetAbilitiesOnLevel(int level)
        {
            return progression[level - 1].abilities;
        }

        public List<Ability> GetAllAbilitesAvaliable(int level)
        {
            List<Ability> abilities = new List<Ability>();
            foreach (var prog in progression)
            {
                foreach (var ability in prog.abilities)
                {
                    abilities.Add(ability);
                }
            }
            return abilities;
        }

        [System.Serializable]
        class ProgressionPerLevel
        {
            public Statistic health = new Statistic(StatType.Health);
            public Statistic strength = new Statistic(StatType.Strength);
            public Statistic agility = new Statistic(StatType.Agility);
            public Statistic defense = new Statistic(StatType.Defense);
            public Statistic intelect = new Statistic(StatType.Intelect);
            public Ability[] abilities;
            public float expNextLevel;

            public float GetStatistic(StatType type)
            {
                switch (type)
                {
                    case StatType.Strength:
                        return strength.value;
                    case StatType.Health:
                        return health.value;
                    case StatType.Agility:
                        return agility.value;
                    case StatType.Intelect:
                        return intelect.value;
                    case StatType.Defense:
                        return defense.value;
                    default:
                        throw new KeyValueMissingException(type.ToString(), this.ToString());
                };
            }
        }

        [System.Serializable]
        class Statistic
        {
            public Statistic(StatType type)
            {
                this.type = type;
            }
            [HideInInspector]
            public StatType type;
            public float value;
        }
    }

    
}
