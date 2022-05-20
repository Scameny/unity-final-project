using Saving;
using Character.Stats;
using System.Linq;
using NaughtyAttributes;
using System.Collections.Generic;
using Character.Abilities;
using UnityEngine;
using Character.Classes;
using Items;

namespace Character.Character
{
    public class Hero : DefaultCharacter, ISaveable
    {
        
        public float exp;
        public Inventory inventory = new Inventory();

        override public float GetStatistic(StatType type)
        {
            return characterClass.GetStatistic(type, level) + gear.GetAdditiveModifier(type) + traits.GetAdditiveModifier(type);
        }

        override public float GetSecondaryStatistic(DamageTypeStat type)
        {
            return gear.GetAdditiveModifier(type) + +traits.GetAdditiveModifier(type);
        }

        public void AddExp()
        {

        }

        #region Debug
        [Button]
        private void LoadAbilities()
        {
            abilitiesAvaliable = new List<AbilityUsable>();
            abilitiesAvaliable.AddRange(characterClass.GetAllAbilitesAvaliable(level).Select(a =>
            {
                return new AbilityUsable(a);
            }));
        }
        #endregion

        #region Saving system

        [System.Serializable]
        struct HeroData
        {
            public string heroClass;
            public int level;
            public float currentHealth;
        }

        public void RestoreState(object state)
        {
            HeroData data = (HeroData)state;
            characterClass = Resources.Load<PlayerClass>(data.heroClass);
            level = data.level;
            currentHealth = data.currentHealth;
        }

        public object CaptureState()
        {
            HeroData data = new HeroData();
            data.heroClass = characterClass.name;
            data.level = level;
            data.currentHealth = currentHealth;
            return data;
        }
        #endregion
    }
}

