using Character.Stats;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Character.Abilities;

namespace Items
{
    [CreateAssetMenu(fileName = "GearItem", menuName = "Items/Type of items/GearItem", order = 2)]
    public class GearItem : Item, IModifierProvider
    {

        public GearPiece slot;
        [EnableIf("slot", GearPiece.Weapon)]
        public AttackDamage attackDamage;
        public GearStat[] stats;
        public GearSecondaryStat[] secondaryStats;


        public IEnumerable<float> GetAdditiveModifier(DamageTypeStat stat)
        {
            foreach (var givenStat in secondaryStats)
            {
                if (givenStat.statType == stat)
                {
                    yield return givenStat.amount;
                }
            }
        }

        public IEnumerable<float> GetAdditiveModifier(StatType stat)
        {
            foreach (var givenStat in stats)
            {
                if (givenStat.statType == stat)
                {
                    yield return givenStat.amount;
                }
            }
        }
    }


    [System.Serializable]
    public class GearStat
    {
        public StatType statType;
        public float amount;
    }

    [System.Serializable]
    public class GearSecondaryStat
    {
        public DamageTypeStat statType;
        public float amount;
    }

    [System.Serializable]
    public class AttackDamage
    {
        public float minimAttack;
        public float maxAttack;
        public StatType scalingStat;
        public float scaleCoef;
        public DamageType damageType;
        public Ability ability;
    }


    public enum GearPiece
    {
        Helm,
        Chest,
        Boots,
        Legs,
        Gloves,
        Ring,
        Weapon
    }

}

