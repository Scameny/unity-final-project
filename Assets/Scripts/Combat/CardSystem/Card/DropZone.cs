using CardSystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public void OnDrop(PointerEventData eventData)
        {
            Card d = eventData.pointerDrag.GetComponent<Card>();
            if (d != null)
            {
                d.OnZoneDropExit();
                d.UseCard();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null)
                return;
            Card d = eventData.pointerDrag.GetComponent<Card>();
            if (d!= null)
            {
                d.OnZoneDropEnter();
            }
    
    }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null)
                return;
            Card d = eventData.pointerDrag.GetComponent<Card>();
            if (d != null)
            {
                d.OnZoneDropExit();
            }
        }
    }

}
