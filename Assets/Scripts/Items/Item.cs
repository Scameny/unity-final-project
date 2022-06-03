using CardSystem;
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
        [ShowAssetPreview]
        [SerializeField] Sprite sprite;

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

        public abstract ItemType GetItemType();

        public Sprite GetSprite()
        {
            return sprite;
        }
    }


    public enum ItemType
    {
        Equipable,
        KeyItem
    }
}
