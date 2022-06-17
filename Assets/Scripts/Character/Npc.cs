using Character.Stats;
using Character.Reward;
using System.Collections.Generic;
using Items;
using CardSystem;

namespace Character.Character
{
    public class Npc : DefaultCharacter {

        public NpcReward reward;

        public bool isDead { get; private set; }

        private void Start()
        {
            currentHealth = GetStatistic(StatType.Health);
            maxHealth = currentHealth;
            LoadAbilities();
        }

        override public float GetStatistic(StatType type)
        {
            return base.GetStatistic(type);
        }

        override public float GetSecondaryStatistic(DamageTypeStat type)
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
        }
    }
}
