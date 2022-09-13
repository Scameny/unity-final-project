using CardSystem;
using Character.Character;
using GameManagement;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;

namespace UI.Combat
{
    public class UICombatCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] Image cardImage;
        [SerializeField] TextMeshProUGUI cardName, cardDescription;
        [SerializeField] GameObject cost, usableAura;
        [SerializeField] Vector3 scaleWhenMouseOver;
        [SerializeField] int YmoveWhenMouseOver;

        bool oneUse;
        Vector3 initialPosition, initialRotation;
        bool onDropZone = false;
        bool isDragging;
        int siblingIndex;
       
        
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
            cardDescription.text = UtilsClass.instance.ConvertTextWithStyles(cardUse.GetDescription(), UIManager.manager.GetTooltipStyle());
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

        public void OnPointerEnter(PointerEventData eventData)
        {
            siblingIndex = transform.GetSiblingIndex();
            transform.SetAsLastSibling();
            transform.localScale = scaleWhenMouseOver;
            transform.localPosition += new Vector3(0, YmoveWhenMouseOver); 
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.localScale = Vector3.one;
            GetComponent<RectTransform>().position = initialPosition;
            transform.SetSiblingIndex(siblingIndex);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            isDragging = true;
            transform.rotation = Quaternion.identity;
            UIManager.manager.SendData(new SignalData(GameSignal.START_DRAGGING_CARD));
        }

        public void OnDrag(PointerEventData eventData)
        {
            GetComponent<RectTransform>().position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            GetComponent<RectTransform>().position = initialPosition;
            transform.rotation = Quaternion.Euler(initialRotation);
            isDragging = false;
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

        public bool IsDragging()
        {
            return isDragging;
        }

        public void SetPosition(Vector3 position)
        {
            initialPosition = position;
        }

        public void SetRotation(Vector3 rotation)
        {
            initialRotation = rotation;
        }
    }

}
