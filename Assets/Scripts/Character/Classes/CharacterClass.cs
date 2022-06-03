using UnityEngine;
using Character.Stats;
using System.Collections.Generic;
using Abilities.ability;
using CardSystem;

namespace Character.Classes
{
    public abstract class CharacterClass : ScriptableObject
    {
        [SerializeField] protected Progression progression = null;

        public float GetStatistic(StatType type, int level)
        {
            return progression.GetStatistic(type, level);
        }

        public List<AbilityCard> GetAllAbilitesAvaliable(int level)
        {
            return progression.GetAllAbilitesAvaliable(level);
        }

        public AbilityCard[] GetAbilitiesOnLevel(int level)
        {
            return progression.GetAbilitiesOnLevel(level);
        }

        public int GetMaxCardsHand(int level)
        {
            return progression.GetMaxCardsHand(level);
        }
    }
}
