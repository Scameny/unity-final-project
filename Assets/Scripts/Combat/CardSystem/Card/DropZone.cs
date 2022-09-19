using CardSystem;
using Combat.Character;
using GameManagement;
using System.Collections.Generic;
using UI.Combat;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {

        private void Start()
        {
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
                    return;
                }
                catch (NotEnoughResourceException e)
                {
                    UIManager.manager.SendData(new ErrorSignalData(GameSignal.NOT_ENOUGH_RESOURCES, new List<string>() { e.resource.ToString() }));
                }
                catch (NotYourTurnException)
                {
                    UIManager.manager.SendData(new ErrorSignalData(GameSignal.NOT_YOUR_TURN, new List<string>()));
                }
                d.GetComponent<Card>().CancelCardUse();
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
