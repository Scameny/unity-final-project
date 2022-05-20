using System.Linq;
using Character.Stats;
using Character.Abilities;
using UnityEngine;

namespace Character.Character
{
    public class Npc : DefaultCharacter {

        public bool isDead { get; private set; }

        private void Start()
        {
            abilitiesAvaliable.AddRange(characterClass.GetAllAbilitesAvaliable(level).Select(a => 
            {
                return new AbilityUsable(a);
            }));
        }

        override public float GetStatistic(StatType type)
        {
            // Falta añadir traits
            return characterClass.GetStatistic(type, level) + gear.GetAdditiveModifier(type);
        }

        override public float GetSecondaryStatistic(DamageTypeStat type)
        {
            // Falta añadir traits
            return gear.GetAdditiveModifier(type);
        }
    }
}
