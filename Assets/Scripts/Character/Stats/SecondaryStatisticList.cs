using Items;
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
    public class SecondaryStatisticList
    {
        [ValueDropdown("CustomAddSecondaryStatsButton", IsUniqueList = true, DrawDropdownForListElements = false, DropdownTitle = "Modify Stats")]
        [ListDrawerSettings(DraggableItems = false, Expanded = true)]
        [LabelText("SecondaryStats")]
        [SerializeField] public List<GearSecondaryStat> stats = new List<GearSecondaryStat>();

        private IEnumerable CustomAddSecondaryStatsButton()
        {
            return Enum.GetValues(typeof(DamageTypeStat)).Cast<DamageTypeStat>()
                .Except(this.stats.Select(x => x.statType))
                .Select(x => new GearSecondaryStat(x))
                .AppendWith(this.stats)
                .Select(x => new ValueDropdownItem(x.statType.ToString(), x));
        }
    }
}
