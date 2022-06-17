using Character.Stats;
using System.Collections.Generic;
using UnityEngine;
using CardSystem;
using Sirenix.OdinInspector;
using System.Collections;
using System;
using System.Linq;
using Sirenix.Utilities;
using Abilities.Passive;
using Character.Trait;

namespace Items
{
    public abstract class GearItem : Item, IModifierProvider, ICardGiver, IPassiveProvider
    {
        [HorizontalGroup("Middle")]

        [VerticalGroup("Middle/Left")]
        [SerializeField] StatisticList statList;

        [VerticalGroup("Middle/Right")]
        [SerializeField] SecondaryStatisticList secondaryStatList;


        [HorizontalGroup("Bottom")]
        [VerticalGroup("Bottom/Left")]
        [ListDrawerSettings(Expanded = true)]
        [PropertySpace(SpaceBefore = 20)]
        [SerializeField] List<UsableCard> abilitiesGiven = new List<UsableCard>();

        [SerializeField] List<Passive> passiveAbilities = new List<Passive>();


        public IEnumerable<float> GetAdditiveModifier(DamageTypeStat stat)
        {
            foreach (var givenStat in secondaryStatList.stats)
            {
                if (givenStat.statType == stat)
                {
                    yield return givenStat.amount;
                }
            }
        }

        public IEnumerable<float> GetAdditiveModifier(StatType stat)
        {
            foreach (var givenStat in statList.stats)
            {
                if (givenStat.statType == stat)
                {
                    yield return givenStat.amount;
                }
            }
        }

        public override ItemType GetItemType()
        {
            return ItemType.Equipable;
        }

       

        public IEnumerable<Usable> GetUsableCards()
        {
            foreach (var item in abilitiesGiven)
            {
                for (int i = 0; i < item.quantity; i++)
                {
                    yield return item.usable;
                }
            }
        }

        public abstract GearPiece GetSlotType();

        public IEnumerable<Passive> GetPasiveAbilities()
        {
            foreach (var item in passiveAbilities)
            {
                yield return item;
            }
        }
    }



    [Serializable]
    public class GearSecondaryStat
    {
        public GearSecondaryStat(DamageTypeStat secondaryStat)
        {
            this.statType = secondaryStat;
        }



        [HideInInspector]
        public DamageTypeStat statType;
        [LabelText("$statType"), LabelWidth(150)]
        public float amount;
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

