using Items;
using UnityEngine;

namespace UI
{
    public class UIGearSlot : UISlot
    {
        [SerializeField] GearSlot slot;


        private void Start()
        {
            draggableItem.SetItem(player.GetItemBySlot(slot));
        }

        protected override void OnDropNewDraggableItem(DraggableItem droppedDraggableItem)
        {
            Item item = droppedDraggableItem.GetItem();
            Item itemDropped = draggableItem.GetItem();
            if (item.GetItemType() == ItemType.Equipable && OnItemChange(item))
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

        public override bool OnItemChange(Item item)
        {
            GearItem itemToEquip = (GearItem) item;
            if (player.SetItemBySlot(slot, itemToEquip))
            {
                draggableItem.SetItem(itemToEquip);
                return true;
            }
            return false;
        }
    }
}