using CardSystem;
using Combat;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        HeroCombat player;

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<HeroCombat>();
        }

        public void OnDrop(PointerEventData eventData)
        {
            Card d = eventData.pointerDrag.GetComponent<Card>();
            if (d != null)
            {
                d.OnZoneDropExit();
                player.AddCardToQueue(d);
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
