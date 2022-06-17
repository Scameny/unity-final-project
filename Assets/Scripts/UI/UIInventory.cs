using Character.Character;
using Items;
using System.Collections.Generic;
using UnityEngine;


namespace UI
{
    public class UIInventory : MonoBehaviour
    {
        Hero player;
        UIInventorySlot[] slots;
        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Hero>();
            slots = GetComponentsInChildren<UIInventorySlot>();
            Item[,] itemsInInventory = player.GetAllStoredItems();
            for (int i = 0; i < itemsInInventory.GetLength(0); i++)
            {
                for (int j = 0; j < itemsInInventory.GetLength(1); j++)
                {
                    if (itemsInInventory[i,j] != null)
                        slots[j * itemsInInventory.GetLength(0) + i].GetDragableItem().SetItem(itemsInInventory[i,j]);
                    slots[j * itemsInInventory.GetLength(0) + i].slotInfo.i = i;
                    slots[j * itemsInInventory.GetLength(0) + i].slotInfo.j = j;
                }
            }
        }



    }

}