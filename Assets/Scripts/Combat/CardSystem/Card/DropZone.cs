using CardSystem;
using Combat.Character;
using GameManagement;
using System.Collections.Generic;
using UI.Combat;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] float alphaOfDropzone;

        public void OnDrop(PointerEventData eventData)
        {
            UICombatCard d = eventData.pointerDrag.GetComponent<UICombatCard>();
            if (d != null)
            {
                OnPointerExit(eventData);
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
                Color color = GetComponent<Image>().color;
                GetComponent<Image>().color = new Color(color.r, color.g, color.b, alphaOfDropzone);
            }
        }


        public void OnPointerExit(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null)
                return;
            UICombatCard d = eventData.pointerDrag.GetComponent<UICombatCard>();
            if (d != null)
            {
                Color color = GetComponent<Image>().color;
                GetComponent<Image>().color = new Color(color.r, color.g, color.b, 0);
            }
        }
    }

}
