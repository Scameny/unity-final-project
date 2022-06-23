using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Strategies.PassiveEffectStrategies
{
    [Serializable]
    public class PassiveEffectStrategyList
    {
        [TypeFilter("GetFilteredPassiveEffectStrategyList")]
        [ListDrawerSettings(Expanded = true)]
        [SerializeReference] List<PassiveEffectStrategy> effectStrategies = new List<PassiveEffectStrategy>();

        public List<PassiveEffectStrategy> GetPassiveEffectStrategies()
        {
            return effectStrategies;
        }

        public IEnumerable<Type> GetFilteredPassiveEffectStrategyList()
        {
            var q = typeof(PassiveEffectStrategy).Assembly.GetTypes()
                .Where(x => !x.IsAbstract)                                          // Excludes BaseClass
                .Where(x => typeof(PassiveEffectStrategy).IsAssignableFrom(x));                 // Excludes classes not inheriting from BaseClass
            return q;
        }
    }
}
