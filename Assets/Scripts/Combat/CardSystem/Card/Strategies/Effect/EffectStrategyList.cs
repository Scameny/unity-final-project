using Sirenix.OdinInspector;
using Strategies.EffectStrategies;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Strategies.EffectStrategies
{
    [Serializable]
    public class EffectStrategyList
    {
        [TypeFilter("GetFilteredEffectStrategyList")]
        [ListDrawerSettings(Expanded = true)]
        [SerializeField] public List<EffectStrategy> effectStrategies = new List<EffectStrategy>();

        public IEnumerable<Type> GetFilteredEffectStrategyList()
        {
            var q = typeof(EffectStrategy).Assembly.GetTypes()
                .Where(x => !x.IsAbstract)                                          // Excludes BaseClass
                .Where(x => typeof(EffectStrategy).IsAssignableFrom(x));                 // Excludes classes not inheriting from BaseClass
            return q;
        }

    }

}
