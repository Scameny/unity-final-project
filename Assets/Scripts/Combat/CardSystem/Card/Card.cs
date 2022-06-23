using Combat;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;

namespace CardSystem
{
    public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {

        [SerializeField] Image cardImage;
        [SerializeField] TextMeshProUGUI cardName, cardDescription;
        [SerializeField] GameObject cost;
        [SerializeField] SimpleTooltipStyle style;

        bool oneUse;
        Usable cardEffect;
        GameObject user;

        Vector3 position;
        bool onDropZone = false;

        public void InitializeCard(Usable cardUse, GameObject user, bool oneUse)
        {
            cardEffect = cardUse;
            cardImage.sprite = cardUse.GetSprite();
            cardName.text = (cardUse).GetName();
            cardDescription.text = UtilsClass.instance.ConvertTextWithStyles((cardUse).GetDescription(), style);
            if (cardUse.GetResourceCosts().Count > 0)
            {
                //TODO add support to more than one resource cost
                cost.GetComponentInChildren<TextMeshProUGUI>().text = cardUse.GetResourceCosts()[0].amount.ToString();
                // TODO add change of resource background depending on resource type
            }
            this.user = user;
            this.oneUse = oneUse;
        }
      
        public void SetVisibility(bool visible)
        {
            gameObject.SetActive(visible);
        }

        public void UseCard()
        {
            try
            {
                cardEffect.Use(user, CombatManager.combatManager.GetCharactersInCombat(), this);
            } 
            catch (NotEnoughResourceException e)
            {
                Debug.Log("Not enough " + e.resource.ToString());
                CancelCardUse();
            }
        }

        public void CardEffectFinished()
        {
            if (oneUse)
            {
                Destroy(gameObject);
            }
            else
            {
                user.GetComponent<TurnCombat>().SendToStack(this);
            }
            
        }

        public void CancelCardUse()
        {
            gameObject.SetActive(true);
        }

        public IEnumerable<CardEffectType> GetCardEffect()
        {
            foreach (var item in cardEffect.GetCardEffectTypes())
            {
                yield return item;
            }
        }

        public List<ResourceCost> GetResourceCost() 
        {
            return cardEffect.GetResourceCosts();
        }

        #region UI management
        public void OnBeginDrag(PointerEventData eventData)
        {
            position = GetComponent<RectTransform>().position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            this.transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!onDropZone)
            {
                this.transform.position = position;
            }
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
        #endregion
    }
}
