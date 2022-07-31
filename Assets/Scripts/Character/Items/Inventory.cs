using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Items {
    [Serializable]
    public class Inventory
    {
        [LabelText("Inventory")]
        [InlineProperty]
        public Item[] items = new Item[24];
        [SerializeField] int coins;

        public void AddItem(Item itemToAdd, int i)
        {
            items[i] = itemToAdd;
        }

        public void AddItem(Item itemToAdd)
        { 
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] == null)
                {
                    items[i] = itemToAdd;
                    return;
                }
            }
            // inventory full;
        }

        public int GetCurrentCoins()
        {
            return coins;
        }

        public bool UseCoins(int coinsUsed)
        {
            if (coinsUsed > coins)
                return false;
            else
            {
                coins -= coinsUsed;
                return true;
            }
        }


        public void RemoveItem(Item item)
        {
           
        }

    }
}