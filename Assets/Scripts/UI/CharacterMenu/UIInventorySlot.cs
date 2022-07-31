using Items;
using UnityEngine;

namespace UI
{
    public class UIInventorySlot : UISlot
    {
        public int position;

        protected override void OnDropNewDraggableItem(DraggableItem droppedDraggableItem)
        {
            Item item = droppedDraggableItem.GetItem();
            Item itemDropped = draggableItem.GetItem();
            if (OnItemChange(item))
            {
                if (!droppedDraggableItem.GetSlot().OnItemChange(itemDropped))
                {
                    Debug.Log("No se ha podido intercambiar los items");
                    OnItemChange(itemDropped);
                }
            }
            else
            {
                Debug.Log("No se puede equipar el objeto");
            }
        }

        public override bool OnItemChange(Item itemToinventory)
        {
            player.AddItem(itemToinventory, position);
            draggableItem.SetItem(itemToinventory);
            return true;
        }
    }

}
