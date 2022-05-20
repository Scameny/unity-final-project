using NaughtyAttributes;
using Strategies.EffectStrategies;
using Strategies.FilterStrategies;
using Strategies.TargetingStrategies;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public abstract class Item : ScriptableObject
    {
        public ItemType type;
        [MinValue(0)]
        public int maxQuantity;

        public override bool Equals(object obj)
        {
            return obj is Item item &&
                   base.Equals(obj) &&
                   name == item.name;
        }

        public override int GetHashCode()
        {
            int hashCode = -1301573508;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(name);
            return hashCode;
        }
    }


    public enum ItemType
    {
        Consumable,
        Equipable,
        KeyItem
    }
}
