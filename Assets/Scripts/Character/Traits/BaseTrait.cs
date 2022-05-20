using Character.Stats;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character.Trait
{
    [CreateAssetMenu(fileName = "Trait", menuName = "Character/Traits/Trait", order = 1)]
    public class BaseTrait : ScriptableObject, IModifierProvider
    {
        public bool IsTemporary;
        [EnableIf("IsTemporary")]
        public int turns;
        public TraitStat[] stats;
        public TreatSecondaryStat[] secondaryStats;

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

        [System.Serializable]
        public class TraitStat
        {
            public StatType statType;
            public float amount;
        }

        [System.Serializable]
        public class TreatSecondaryStat
        {
            public DamageTypeStat statType;
            public float amount;
        }
    }

}
