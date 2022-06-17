using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Character.Stats 
{
    [Serializable]
    [InlineProperty, HideLabel]
    public class StatisticList
    {
        [ValueDropdown("CustomAddStatsButton", IsUniqueList = true, DrawDropdownForListElements = false, DropdownTitle = "Modify Stats")]
        [ListDrawerSettings(DraggableItems = false, Expanded = true)]
        [SerializeField] public List<StatisticGiven> stats = new List<StatisticGiven>();
        
        private IEnumerable CustomAddStatsButton()
        {
            return Enum.GetValues(typeof(StatType)).Cast<StatType>()
                .Except(this.stats.Select(x => x.statType))
                .Select(x => new StatisticGiven(x))
                .AppendWith(this.stats)
                .Select(x => new ValueDropdownItem(x.statType.ToString(), x));
        }
    }

}
