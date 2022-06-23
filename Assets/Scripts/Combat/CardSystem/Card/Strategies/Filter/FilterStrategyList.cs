using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Strategies.FilterStrategies
{
    [Serializable]
    public class FilterStrategyList
    {
        [TypeFilter("GetFilteredFilterStrategyList")]
        [ListDrawerSettings(Expanded = true)]
        [SerializeReference] public List<IFilterStrategy> filterStrategies = new List<IFilterStrategy>();

        public IEnumerable<Type> GetFilteredFilterStrategyList()
        {
            var q = typeof(IFilterStrategy).Assembly.GetTypes()
                .Where(x => !x.IsAbstract)                                          // Excludes BaseClass
                .Where(x => typeof(IFilterStrategy).IsAssignableFrom(x));                 // Excludes classes not inheriting from BaseClass
            return q;
        }

    }

}
