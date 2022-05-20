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

        [System.Serializable]
        public class ItemsDictionary : SerializableDictionaryBase<Item, int> { };

        public ItemsDictionary gearItems;
        public ItemsDictionary consumablesItems;
        public ItemsDictionary keyItems;

        public void UseItem(Item item, GameObject user, List<GameObject> targets)
        {
            if (item.type== ItemType.Consumable)
            {
                ((UsableItem)item).Use(user, targets);
                if (item.type == ItemType.Consumable)
                    RemoveItem(item, 1);
            }
            else
            {
                GameDebug.Instance.LogError("Trying to use item not usable");
            }
        }

        public List<Item> GetListOfItems(ItemType type)
        {
            switch (type)
            {
                case ItemType.Consumable:
                    return new List<Item>(consumablesItems.Keys);
                case ItemType.Equipable:
                    return new List<Item>(gearItems.Keys);
                case ItemType.KeyItem:
                    return new List<Item>(keyItems.Keys);
            }
            throw new KeyValueMissingException(type.ToString(), GetType().Name);
        }

        public void AddItem(Item item, int quantity)
        {
            switch (item.type)
            {
                case ItemType.Consumable:
                    UpdateDictionary(item, quantity, consumablesItems);
                    break;
                case ItemType.Equipable:
                    UpdateDictionary(item, quantity, gearItems);
                    break;
                case ItemType.KeyItem:
                    UpdateDictionary(item, quantity, keyItems);
                    break;
            }
        }

        public bool ExistItem(Item item)
        {
            switch (item.type)      
            {
                case ItemType.Consumable:
                    return consumablesItems.ContainsKey(item);
                case ItemType.Equipable:
                    return gearItems.ContainsKey(item);
                case ItemType.KeyItem:
                    return keyItems.ContainsKey(item);
            }
            throw new KeyValueMissingException(item.type.ToString(), this.ToString());
        }

        public void RemoveItem(Item item, int quantity)
        {
            switch (item.type)
            {
                case ItemType.Consumable:
                    UpdateDictionary(item, -quantity, consumablesItems);
                    break;
                case ItemType.Equipable:
                    UpdateDictionary(item, -quantity, gearItems);
                    break;
                case ItemType.KeyItem:
                    UpdateDictionary(item, -quantity, keyItems);
                 break;
            }
        }

        private void UpdateDictionary(Item item, int quantity, ItemsDictionary dic)
        {
            if (!dic.ContainsKey(item))
            {
                if (quantity < 1)
                    throw new NotValidOperationException(MethodBase.GetCurrentMethod().Name, this.ToString());
                dic.Add(item, quantity);
            } else
            {
                dic[item] += quantity;
                dic[item] = Mathf.Max(0, dic[item], Mathf.Min(item.maxQuantity, dic[item]));
            }

            if (dic[item] == 0)
                dic.Remove(item);

        }
    }
}