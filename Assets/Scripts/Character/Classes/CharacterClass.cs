using UnityEngine;
using Character.Stats;
using System.Collections.Generic;
using CardSystem;
using Abilities.Passive;

namespace Character.Classes
{
    public abstract class CharacterClass : ScriptableObject
    {
        [SerializeField] protected Progression progression;
        [SerializeField] protected ResourceType resourceType;

        public int GetStatistic(StatType type, int level)
        {
            return progression.GetStatistic(type, level);
        }

        public IEnumerable<Passive> GetPassiveAbilitiesOnLevel(int level)
        {
            foreach (var item in progression.GetPassiveAbilitiesOnLevel(level))
            {
                yield return item;
            }
        }

        public IEnumerable<Passive> GetAllPassiveAbilitiesAvaliable(int level)
        {
            foreach (var item in progression.GetAllPassiveAbilitiesAvaliable(level))
            {
                yield return item;
            }
        }


        public IEnumerable<Usable> GetAllAbilitesAvaliable(int level)
        {
            return progression.GetAllAbilitesAvaliable(level);
        }

        public IEnumerable<Usable> GetAbilitiesOnLevel(int level)
        {
            return progression.GetAbilitiesOnLevel(level);
        }

        public int GetMaxCardsHand(int level)
        {
            return progression.GetMaxCardsHand(level);
        }

        public ResourceType GetResourceType()
        {
            return resourceType;
        }
    }
}
