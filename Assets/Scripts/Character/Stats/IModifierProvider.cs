using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Character.Stats 
{
    public interface IModifierProvider
    {
        IEnumerable<float> GetAdditiveModifier(StatType stat);

        IEnumerable<float> GetAdditiveModifier(DamageTypeStat stat);

    }


}
