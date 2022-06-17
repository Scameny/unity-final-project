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
        [SerializeField] public List<FilterStrategy> filterStrategies = new List<FilterStrategy>();

        public IEnumerable<Type> GetFilteredFilterStrategyList()
        {
            var q = typeof(FilterStrategy).Assembly.GetTypes()
                .Where(x => !x.IsAbstract)                                          // Excludes BaseClass
                .Where(x => typeof(FilterStrategy).IsAssignableFrom(x));                 // Excludes classes not inheriting from BaseClass
            return q;
        }

    }

}
