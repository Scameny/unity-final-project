using Character.Character;
using Combat;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CardSystem
{
    public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {

        [SerializeField] Image cardImage;
        [SerializeField] Text cardName;


        bool temporary;
        IUsable cardEffect;
        GameObject user;

        Vector3 position;
        bool onDropZone = false;

        public void InitializeCard(IUsable cardUse, GameObject user, bool temporary)
        {
            cardEffect = cardUse;
            cardImage.sprite = cardUse.GetSprite();
            cardName.text = ((ScriptableObject)cardUse).name;
            this.temporary = temporary;
            this.user = user;
        }
      
        public void SetVisibility(bool visible)
        {
            gameObject.SetActive(visible);
        }

        public void UseCard()
        {
            cardEffect.Use(user, CombatManager.combatManager.GetCharactersInCombat(), this);
        }

        public void CardEffectFinished()
        {
            user.GetComponent<TurnCombat>().SendToStack(this);
        }

        public void CancelCardUse()
        {
            gameObject.SetActive(true);
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
