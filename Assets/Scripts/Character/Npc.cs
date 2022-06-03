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
        }

        override public float GetStatistic(StatType type)
        {
            // Falta añadir traits
            return characterClass.GetStatistic(type, level) + traits.GetAdditiveModifier(type);
        }

        override public float GetSecondaryStatistic(DamageTypeStat type)
        {
            // Falta añadir traits
            return traits.GetAdditiveModifier(type);
        }

        public int GetRewardExp()
        {
            return reward.exp;
        }

        public List<Item> GetRewardItems()
        {
            return reward.GetLoot();
        }
    }
}
