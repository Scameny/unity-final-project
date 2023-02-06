using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Character.Stats
{
    [Serializable]
    public class SecondaryStatisticGiven
    {
        public SecondaryStatisticGiven(StatType statType)
        {
            this.statType = statType;
        }

        [HideInInspector]
        public StatType statType;
        [LabelText("$statType"), LabelWidth(150)]
        public float amount;
    }
}
