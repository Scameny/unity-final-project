using Items;
using RotaryHeart.Lib.SerializableDictionary;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Utils;

namespace Items { 
    [System.Serializable]
    public class Inventory
    {
        public List<Item> gearItems;
        public List<Item> keyItems;

        public void AddItem(Item item)
        {
            switch (item.GetItemType())
            {
                case ItemType.Consumable:
                    break;
                case ItemType.Equipable:
                    gearItems.Add(item);
                    break;
                case ItemType.KeyItem:
                    keyItems.Add(item);
                    break;
                default:
                    break;
            }
        }

        public void RemoveItem(Item item, int quantity)
        {
            
        }

    }
}