using Character.Character;
using Items;
using System.Collections.Generic;
using UnityEngine;


namespace UI
{
    public class UIInventory : MonoBehaviour
    {
        Hero player;
        UISlot[] slots;
        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Hero>();
            slots = GetComponentsInChildren<UISlot>();
            int count = 0;
            List<Item> itemsInInventory = player.GetAllStoredItems();  
            foreach (var item in itemsInInventory)
            {
                slots[count].GetDragableItem().SetItem(item);
                count++;
            }
        }



    }

}