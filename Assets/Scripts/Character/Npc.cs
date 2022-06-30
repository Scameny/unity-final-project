using Character.Stats;
using Character.Reward;
using System.Collections.Generic;
using Items;
using CardSystem;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Character.Character
{
    public class Npc : DefaultCharacter {

        public NpcReward reward;
        [MinValue(0.1), MaxValue(1.0)]
        [SerializeField] float iaPercentageForHealing = 0.2f;

        public bool isDead { get; private set; }

        override protected void Start()
        {
            base.Start();
            LoadAbilities();
        }

        override public int GetStatistic(StatType type)
        {
            return base.GetStatistic(type);
        }

        override public int GetSecondaryStatistic(DamageTypeStat type)
        {
            return base.GetSecondaryStatistic(type);
        }

        public int GetRewardExp()
        {
            return reward.exp;
        }

        public List<Item> GetRewardItems()
        {
            return reward.GetLoot();
        }

        public void LoadAbilities()
        {

            foreach (var item in GetAllClassAbilitiesAvaliable())
            {
                permanentCards.Add(item);
            }
            foreach (var item in GetClassPasiveAbilitiesAvaliable())
            {
                permanentPassiveAbilities.Add(item);
            }
        }

        #region Getters

        public float GetIAPercentageForHealing()
        {
            return iaPercentageForHealing;
        }

        #endregion
    }
}
