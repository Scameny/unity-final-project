using Character.Stats;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Abilities.ability;
using CardSystem;
using Abilities.BasicAttack;

namespace Items
{
    [CreateAssetMenu(fileName = "GearItem", menuName = "Items/Type of items/GearItem", order = 2)]
    public class GearItem : Item, IModifierProvider, ICardGiver
    {
        ItemType type = ItemType.Equipable;

        public GearPiece slot;
        [EnableIf("slot", GearPiece.Weapon)]
        [SerializeField] AttackDamage attackDamage;
        [SerializeField] GearStat[] stats;
        [SerializeField] GearSecondaryStat[] secondaryStats;
        [SerializeField] UsableCard[] abilitiesGiven;


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

        public override ItemType GetItemType()
        {
            return type;
        }

        public IEnumerable<UsableCard> GetUsableCards()
        {
            foreach (var item in abilitiesGiven)
            {
                yield return item;
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
    }


    public enum GearPiece
    {
        Helm,
        Chest,
        Legs,
        Gloves,
        Ring,
        Weapon
    }

}

