using CardSystem;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class UICard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] Image cardImage;
        [SerializeField] TextMeshProUGUI cardName, cardDescription;
        [SerializeField] GameObject cost;

        Usable usable;
        Vector3 initScale;
        bool interactable;
        
        public void InitializeCard(Usable cardUse, bool interactable = true)
        {
            this.interactable = interactable;
            this.usable = cardUse;
            cardImage.sprite = cardUse.GetSprite();
            cardName.text = (cardUse).GetName();
            cardDescription.text = UtilsClass.instance.ConvertTextWithStyles(cardUse.GetDescription(), UIManager.manager.tooltipStyle);
            if (cardUse.GetResourceCosts().Count > 0)
            {
                //TODO add support to more than one resource cost
                cost.GetComponentInChildren<TextMeshProUGUI>().text = cardUse.GetResourceCosts()[0].amount.ToString();
                // TODO add change of resource background depending on resource type
            }
        }

        public Usable GetCardUse()
        {
            return usable;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!interactable)
                return;
            initScale = transform.localScale;
            transform.localScale = 1.3f * initScale;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!interactable)
                return;
            transform.localScale = initScale;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!interactable)
                return;
            transform.parent.GetComponent<UICardMenu>().OnSelection(usable);
        }
    }
}

