using CardSystem;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;

namespace UI.Cards
{
    public class UICard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] Image cardImage;
        [SerializeField] TextMeshProUGUI cardName, cardDescription;
        [SerializeField] GameObject cost;
        [SerializeField] float scaleWhenSelected;

        Usable usable;
        Vector3 initScale;
        bool interactable;
        
        public void InitializeCard(Usable cardUse, bool interactable = true)
        {
            this.interactable = interactable;
            usable = cardUse;
            cardImage.sprite = cardUse.GetSprite();
            cardName.text = (cardUse).GetName();
            cardDescription.text = UtilsClass.instance.ConvertTextWithStyles(cardUse.GetDescription(), UIManager.manager.GetTooltipStyle());
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

        public void SetScaleWhenSelected(float scale)
        {
            scaleWhenSelected = scale;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!interactable)
                return;
            initScale = transform.localScale;
            transform.localScale = scaleWhenSelected * initScale;
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
            transform.GetComponentInParent<UICardMenu>().OnSelection(usable);
        }
    }
}

