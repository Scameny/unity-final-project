using Sirenix.OdinInspector;
using System;

namespace Items {
    [Serializable]
    public class Inventory
    {
        [LabelText("Inventory")]
        [InlineProperty]
        public Item[,] items = new Item[8,3];

        public void AddItem(Item itemToAdd, int i, int j)
        {
            items[i, j] = itemToAdd;
        }

        public void AddItem(Item itemToAdd)
        {
            for (int i = 0; i < items.GetLength(0); i++)
            {
                for (int j = 0; j < items.GetLength(1); j++)
                {
                    if (items[i,j] == null)
                    {
                        items[i, j] = itemToAdd;
                        return;
                    }
                }
            }
            // inventory full;
        }


        public void RemoveItem(Item item)
        {
           
        }

    }
}