using CardSystem;
using Combat;
using UI.Combat;
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
            UICombatCard d = eventData.pointerDrag.GetComponent<UICombatCard>();
            if (d != null)
            {
                d.OnZoneDropExit();
                try 
                {
                    d.UseCard();
                } catch
                {
                    d.GetComponent<Card>().CancelCardUse();
                }
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null)
                return;
            UICombatCard d = eventData.pointerDrag.GetComponent<UICombatCard>();
            if (d!= null)
            {
                d.OnZoneDropEnter();
            }
    
    }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null)
                return;
            UICombatCard d = eventData.pointerDrag.GetComponent<UICombatCard>();
            if (d != null)
            {
                d.OnZoneDropExit();
            }
        }
    }

}
