using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Character.Stats
{
    [Serializable]
    public class StatisticGiven
    {
        public StatisticGiven(StatType statType)
        {
            this.statType = statType;
        }

        [HideInInspector]
        public StatType statType;
        [LabelText("$statType"), LabelWidth(150)]
        public float amount;
    }
}