using UnityEngine;
using Character.Stats;
using Character.Abilities;
using System.Collections.Generic;
using NaughtyAttributes;

namespace Character.Classes
{
    public abstract class CharacterClass : ScriptableObject
    {
        [SerializeField] Progression progression = null;

        public float GetStatistic(StatType type, int level)
        {
            return progression.GetStatistic(type, level);
        }

        public List<Ability> GetAllAbilitesAvaliable(int level)
        {
            return progression.GetAllAbilitesAvaliable(level);
        }

        public Ability[] GetAbilitiesOnLevel(int level)
        {
            return progression.GetAbilitiesOnLevel(level);
        }
    }
}
