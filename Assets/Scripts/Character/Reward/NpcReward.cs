using Items;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Character.Reward {

    [System.Serializable]
    public class NpcReward
    {
        public int exp;
        [MaxValue(100.0f)]
        [MinValue(0.0f)]
        public float lootProb;
        public int maxItems;
        public List<LootItemInfo> lootList= new List<LootItemInfo>();


        public List<Item> GetLoot()
        {
            List<Item> itemsDropped = new List<Item>();
            float dropLoot = Random.Range(0.0f, 1.0f);
            if (lootProb > dropLoot)
            {
                int numItemsDropped = Random.Range(1, maxItems);
                while (itemsDropped.Count < numItemsDropped)
                {
                    float dropNum = Random.Range(0.0f, 100.0f);
                    foreach (var loot in lootList)
                    {
                        if (dropNum > loot.dropChances)
                        {
                            itemsDropped.Add(loot.item);
                        }
                    }
                }
            }
            return itemsDropped;
        }
    }

    

    [System.Serializable]
    public class LootItemInfo
    {
        public Item item;
        public int quantity;
        [MaxValue(100.0f)]
        [MinValue(0.01f)]
        public int dropChances;
    }
}
