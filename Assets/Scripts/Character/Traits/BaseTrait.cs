using Abilities.Passive;
using CardSystem;
using Character.Stats;
using Items;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Character.Trait
{
    [System.Serializable]
    public class BaseTrait : IModifierProvider, ICardGiver, IPassiveProvider
    {
        [HorizontalGroup("Main")]

        [BoxGroup("Main/General Settings")]
        [SerializeField] string name;
        [HorizontalGroup("Main/General Settings/Split")]
        [VerticalGroup("Main/General Settings/Split/Left")]
        [SerializeField] bool temporary;
        [ShowIf("IsTemporary")]
        [HorizontalGroup("Main/General Settings/Split")]
        [VerticalGroup("Main/General Settings/Split/Right")]
        [LabelWidth(60)]
        [SerializeField] int turns;
        public Sprite icon;

        [HorizontalGroup("Middle")]

        [VerticalGroup("Middle/Left")]
        [SerializeField] StatisticList statList = new StatisticList();
        
        
        [VerticalGroup("Middle/Left")]
        [SerializeField] SecondaryStatisticList secondaryStatList = new SecondaryStatisticList();


        [ListDrawerSettings(Expanded = true)]
        [SerializeField] List<UsableCard> cards = new List<UsableCard>();


        [SerializeField] List<Passive> passiveList = new List<Passive>();



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

        public IEnumerable<Usable> GetUsableCards()
        {
            foreach (var card in cards)
            {
                for (int i = 0; i < card.quantity; i++)
                {
                    yield return card.usable;
                }
            }
        }
        
        public IEnumerable<Passive> GetPasiveAbilities()
        {
            foreach (var item in passiveList)
            {
                yield return item;
            }
        }

        #region getters
        
        public string GetName()
        {
            return name;
        }

        public int GetTurns()
        {
            return turns;
        }

        public bool IsTemporary()
        {
            return temporary;
        }

        public IEnumerable<StatisticGiven> GetStatistic()
        {
            return statList.stats;
        }

        public IEnumerable<GearSecondaryStat> GetSecondaryStatistic()
        {
            return secondaryStatList.stats;
        }

        public IEnumerable<UsableCard> GetCards()
        {
            return cards;
        }
        #endregion

        #region Object operations
        public override bool Equals(object obj)
        {
            return obj is BaseTrait trait &&
                   name == trait.name;
        }

        public override int GetHashCode()
        {
            return 363513814 + EqualityComparer<string>.Default.GetHashCode(name);
        }
        #endregion
    }

}
