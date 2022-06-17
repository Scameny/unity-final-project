using Abilities.Passive;
using CardSystem;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using System.Collections.Generic;
using UnityEngine;

namespace Character.Stats
{
    [System.Serializable]
    public class Progression
    {
        [SerializeField] protected BaseStats baseStats;
        [ListDrawerSettings(NumberOfItemsPerPage = 1, Expanded = true, ListElementLabelName = "label", CustomAddFunction ="CustomAddFunction")]
        [SerializeField] List<CharacterStatsPerLevel> progression = new List<CharacterStatsPerLevel>();

        public int GetStatistic(StatType statType, int level)
        {
            int toRet = 0;
            toRet += baseStats.stats.GetStatistic(statType);
            for (int i = 0; i < level; i++)
            {
                toRet += progression[i].stats.GetStatistic(statType);
            }
            return toRet;
        }

        public int GetExpNeeded(int level)
        {
            return progression[level - 1].expNextLevel;
        }

        public int GetMaxCardsHand(int level)
        {
            int toRet = 0;
            toRet += baseStats.maxCards;
            for (int i = 0; i < level; i++)
            {
                toRet += progression[level - 1].newSlotsHand;
            }
            return toRet; ;
        }

        public IEnumerable<Usable> GetAbilitiesOnLevel(int level)
        {
            return progression[level - 1].stats.GetUsableCards();
        }

        public IEnumerable<Usable> GetAllAbilitesAvaliable(int level)
        {
            foreach (var item in baseStats.stats.GetUsableCards())
            {
                yield return item;
            }
            for (int i = 0; i < level; i++)
            {
                foreach (var ability in progression[i].stats.GetUsableCards())
                {
                    yield return ability;
                }
            }
        }

        public IEnumerable<Passive> GetPassiveAbilitiesOnLevel(int level)
        {
            return progression[level - 1].stats.GetPassiveAbilities();
        }

        public IEnumerable<Passive> GetAllPassiveAbilitiesAvaliable(int level)
        {
            foreach (var item in baseStats.stats.GetPassiveAbilities())
            {
                yield return item;
            }
            for (int i = 0; i < level; i++)
            {
                foreach (var ability in progression[i].stats.GetPassiveAbilities())
                {
                    yield return ability;
                }
            }
        }

        public int GetMaxLevel()
        {
            return progression.Count;
        }

        private void CustomAddFunction()
        {
            progression.Add(new CharacterStatsPerLevel(progression.Count + 1));
            
        }

        [System.Serializable]
        public class CharacterStats : ICardGiver
        {
            [HorizontalGroup("Top")]

            [BoxGroup("Top/Statistic")]
            [HorizontalGroup("Top/Statistic/Split")]
            [VerticalGroup("Top/Statistic/Split/Left")]
            [LabelWidth(120)]
            public int health;
            [VerticalGroup("Top/Statistic/Split/Left")]
            [LabelWidth(120)]
            public int strength;
            [VerticalGroup("Top/Statistic/Split/Left")]
            [LabelWidth(120)]
            public int agility;
            [VerticalGroup("Top/Statistic/Split/Right")]
            [LabelWidth(120)]
            public int defense;
            [VerticalGroup("Top/Statistic/Split/Right")]
            [LabelWidth(120)]
            public int intelect;
            [HorizontalGroup("Top/Statistic/Bottom")]
            public int resourceAmount;


            [Space(15)]
            [ListDrawerSettings(Expanded = true)]
            public List<UsableCard> abilities;

            [Space(15)]
            [ListDrawerSettings(Expanded = true)]
            public List<Passive> passives;


            public int GetStatistic(StatType type)
            {
                switch (type)
                {
                    case StatType.Strength:
                        return strength;
                    case StatType.Health:
                        return health;
                    case StatType.Agility:
                        return agility;
                    case StatType.Intelect:
                        return intelect;
                    case StatType.Defense:
                        return defense;
                    default:
                        throw new KeyValueMissingException(type.ToString(), this.ToString());
                };
            }

            public IEnumerable<Usable> GetUsableCards()
            {
                foreach (var item in abilities)
                {
                    for (int i = 0; i < item.quantity; i++)
                    {
                        yield return item.usable;
                    }
                };
            }

            public IEnumerable<Passive> GetPassiveAbilities()
            {
                foreach (var item in passives)
                {
                    yield return item;
                }
            }
        }

        [System.Serializable]
        public struct BaseStats
        {
            [InlineProperty, HideLabel]
            [ValidateInput("@stats.abilities.Count > 0", "A class can have base cards list empty")]
            public CharacterStats stats;
            public int maxCards;
        }

        [System.Serializable]
        public class CharacterStatsPerLevel
        {
            public CharacterStatsPerLevel(int level)
            {
                label = "Level " + level;
            }

            [InlineProperty]
            [HideLabel]
            public CharacterStats stats;
            public int newSlotsHand;
            public int expNextLevel;
            [HideInInspector]
            public string label;
        }
    }

}
