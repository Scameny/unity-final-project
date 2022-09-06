using CardSystem;
using Character.Character;
using GameManagement;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;

namespace UI.Cards
{
    public class UICombatCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] Image cardImage;
        [SerializeField] TextMeshProUGUI cardName, cardDescription;
        [SerializeField] GameObject cost, usableAura;

        bool oneUse;
        Vector3 position;
        bool onDropZone = false;
       
        
        Card card;
       
        
        // Start is called before the first frame update
        void Start()
        {
            card = GetComponent<Card>();
        }

        public void InitializeUICard(Usable cardUse, bool oneUse)
        {
            cardImage.sprite = cardUse.GetSprite();
            cardName.text = cardUse.GetName();
            cardDescription.text = UtilsClass.instance.ConvertTextWithStyles(cardUse.GetDescription(), UIManager.manager.tooltipStyle);
            if (cardUse.GetResourceCosts().Count > 0)
            {
                //TODO add support to more than one resource cost
                cost.GetComponentInChildren<TextMeshProUGUI>().text = cardUse.GetResourceCosts()[0].amount.ToString();
                // TODO add change of resource background depending on resource type
            }
            this.oneUse = oneUse;
        }
        
        public void UseCard()
        {
            gameObject.SetActive(false);
            card.UseCard();
        }

        public void CancelCardUse()
        {
            gameObject.SetActive(true);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            position = GetComponent<RectTransform>().position;
            UIManager.manager.SendData(new SignalData(GameSignal.START_DRAGGING_CARD));
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!onDropZone)
            {
                transform.position = position;
            }
            UIManager.manager.SendData(new SignalData(GameSignal.END_DRAGGING_CARD));
        }

        public void OnZoneDropEnter()
        {
            Color color = GetComponent<Image>().color;
            GetComponent<Image>().color = new Color(color.r, color.g, color.b, 0.6f);
        }

        public void OnZoneDropExit()
        {
            Color color = GetComponent<Image>().color;
            GetComponent<Image>().color = new Color(color.r, color.g, color.b, 1);
        }


        private void CanUseCard()
        {
            foreach (var item in card.GetResourceCost())
            {
                if (card.GetUser().GetComponent<DefaultCharacter>().GetCurrentResource(item.resourceType) < item.amount)
                    usableAura.SetActive(false);
            }
            usableAura.SetActive(true);
        }
    }

}
