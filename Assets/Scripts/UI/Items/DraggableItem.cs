using Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        
        Item item;

        // Components in this game object
        Image itemImage;
        Canvas parentCanvas;
        SimpleTooltip tooltip;

        // auxiliar variables, 
        Vector3 initialPosition;
        Transform initialParent;


        private void Awake()
        {
            itemImage = GetComponent<Image>();
            parentCanvas = GetComponentInParent<Canvas>();
            tooltip = GetComponent<SimpleTooltip>();
        }

        public Item GetItem()
        {
            return item;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            initialPosition = GetComponent<RectTransform>().position;
            initialParent = transform.parent;
            transform.SetParent(parentCanvas.transform, true);
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (item != null)
            {
                transform.position = eventData.position;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            transform.position = initialPosition;
            transform.SetParent(initialParent, true);
        }

        public void SetItem(Item item)
        {
            this.item = item;
            if (item != null)
            {
                itemImage.enabled = true;
                itemImage.sprite = item.GetSprite();
                EnableTooltip();
            }
            else
            {
                itemImage.enabled = false;
                DisableTooltip();
            }
        }

        public UISlot GetSlot()
        {
            return initialParent.GetComponent<UISlot>();
        }

        private void EnableTooltip()
        {
            tooltip.enabled = true;
            item.SetTooltipText(tooltip);
        }

        private void DisableTooltip()
        {
            tooltip.enabled = false;
        }

     
    }
}