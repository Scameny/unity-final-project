using Character.Stats;
using System.Collections.Generic;
using UnityEngine;
using CardSystem;
using Sirenix.OdinInspector;
using System;
using Abilities.Passive;
using Character.Buff;
using GameManagement;

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


        public IEnumerable<int> GetAdditiveModifier(DamageTypeStat stat)
        {
            foreach (var givenStat in secondaryStatList.stats)
            {
                if (givenStat.statType == stat)
                {
                    yield return givenStat.amount;
                }
            }
        }

        public IEnumerable<int> GetAdditiveModifier(StatType stat)
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

        override public void SetTooltipText(SimpleTooltip tooltip)
        {
            tooltip.infoRight = GetSlotType().ToString();
            string leftTooltipText= GetName();
            leftTooltipText += "\n@statistic@";
            foreach (var item in statList.stats)
            {
                leftTooltipText += "+ " + item.amount + " " + item.statType.ToString() + "\n";
            }
            foreach (var item in secondaryStatList.stats)
            {
                leftTooltipText += "+ " + item.amount + " " + item.statType.ToString() + "\n";
            }
            leftTooltipText += "@break@\n";
            foreach (var item in abilitiesGiven)
            {
                leftTooltipText += "Equip: (" + item.quantity + ") " + item.usable.GetName() + " - " + item.usable.GetDescription() + "\n";
            }
            foreach (var item in passiveAbilities)
            {
                leftTooltipText += "Equip: " + item.passiveAbility.GetName() + " - " + item.passiveAbility.GetDescription() + "\n";
            }
            tooltip.infoLeft = leftTooltipText;
        }

        public List<SignalData> GetSignalDatas(GameObject user)
        {
            List<SignalData> signalDatas = new List<SignalData>();
            if (statList.stats.Count > 0)
            {
                foreach (var item in statList.stats)
                {
                    signalDatas.Add(new ModifyPrimaryStatisticSignalData(GameSignal.PRIMARY_STAT_MODIFY, user, item.statType));
                }
            }
            if (secondaryStatList.stats.Count > 0)
            {
                foreach (var item in secondaryStatList.stats)
                {
                    signalDatas.Add(new ModifySecondaryStatisticSignalData(GameSignal.PRIMARY_STAT_MODIFY, user, item.statType));
                }
            }

            return signalDatas;
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
        public int amount;
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

