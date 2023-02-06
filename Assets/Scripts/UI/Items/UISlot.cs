using Character.Character;
using Items;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Items
{
    public abstract class UISlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [HideInInspector]
        protected DraggableItem draggableItem;
        protected Hero player;

        private void Awake()
        {
            draggableItem = GetComponentInChildren<DraggableItem>();
        }

        virtual protected void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Hero>();
        }


        public void OnDrop(PointerEventData eventData)
        {
            DraggableItem d = eventData.pointerDrag.GetComponent<DraggableItem>();
            if (d != null)
            {
                OnDropNewDraggableItem(d);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        { 
            if (eventData.pointerDrag == null)
                return;
            DraggableItem d = eventData.pointerDrag.GetComponent<DraggableItem>();
            if (d != null)
            {

            }

        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null)
                return;
            DraggableItem d = eventData.pointerDrag.GetComponent<DraggableItem>();
            if (d != null)
            {

            }
        }

        public DraggableItem GetDragableItem()
        {
            return draggableItem;
        }

        abstract protected void OnDropNewDraggableItem(DraggableItem draggableItem);

        abstract public bool OnItemChange(Item item);
    }


}
