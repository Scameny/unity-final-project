using Abilities.ability;
using CardSystem;
using System.Collections.Generic;
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

        public int GetExpNeeded(int level)
        {
            return progression[level - 1].expNextLevel;
        }

        public int GetMaxCardsHand(int level)
        {
            return progression[level - 1].maxCardsInHand;
        }

        public AbilityCard[] GetAbilitiesOnLevel(int level)
        {
            return progression[level - 1].abilities;
        }

        public List<AbilityCard> GetAllAbilitesAvaliable(int level)
        {
            List<AbilityCard> abilities = new List<AbilityCard>();
            for (int i = 0; i < level; i++)
            {
                foreach (var ability in progression[i].abilities)
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
            public AbilityCard[] abilities;
            public int maxCardsInHand;
            public int expNextLevel;

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
