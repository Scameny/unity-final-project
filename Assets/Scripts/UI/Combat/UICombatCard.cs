using Animations;
using CardSystem;
using Combat;
using GameManagement;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;

namespace UI.Combat
{
    /// <summary>
    /// Class that manages all the UI behaviours of the cards
    /// </summary>
    public class UICombatCard : MonoBehaviour, IObserver<SignalData>, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] Image cardImage, backImage;
        [SerializeField] RawImage usableAura;
        [SerializeField] TextMeshProUGUI cardName, cardDescription;
        [SerializeField] GameObject cost;
        [SerializeField] Vector3 scaleWhenMouseOver;
        [SerializeField] int YmoveWhenMouseOver;

        Vector3 initialPosition, initialRotation;
        bool isDragging, onAnimation;
        int siblingIndex;
        ICardContainer sourceUsed;
        IDisposable disposable;
        Card card;


        // Start is called before the first frame update
        void Start()
        {
            card = GetComponent<Card>();
            disposable = UIManager.manager.Subscribe(this);
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
        }

        public void UseCard()
        {
            isDragging = false;
            usableAura.enabled = false;
            gameObject.SetActive(false);
            sourceUsed = transform.GetComponentInParent<ICardContainer>();
            siblingIndex = sourceUsed.GetIndex(card);
            transform.GetComponentInParent<ICardContainer>().RemoveCard(card);
            UIManager.manager.SendData(new CombatCardSignalData(GameSignal.CARD_PLAYED, card.GetUser(), CombatManager.combatManager.GetCharactersInCombat(), card));
            card.UseCard();
        }

        public void SetCardBack(bool back)
        {
            backImage.enabled = back;
        }

        public void CancelCardUse()
        {
            sourceUsed.AddCard(card, siblingIndex);
            sourceUsed = null;
            usableAura.enabled = true;
            gameObject.SetActive(true);
            UIManager.manager.SendData(new CombatCardSignalData(GameSignal.CARD_PLAYED_CANCEL, card.GetUser(), CombatManager.combatManager.GetCharactersInCombat(), card));
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!CanInteract())
                return;
            siblingIndex = transform.GetSiblingIndex();
            transform.SetAsLastSibling();
            transform.localScale = scaleWhenMouseOver;
            transform.localPosition += new Vector3(0, YmoveWhenMouseOver);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!CanInteract())
                return;
            transform.localScale = Vector3.one;
            GetComponent<RectTransform>().position = initialPosition;
            transform.SetSiblingIndex(siblingIndex);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!CanInteract())
                return;
            isDragging = true;
            transform.rotation = Quaternion.identity;
            UIManager.manager.SendData(new SignalData(GameSignal.START_DRAGGING_CARD));
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!CanInteract())
                return;
            GetComponent<RectTransform>().position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!CanInteract())
                return;
            GetComponent<RectTransform>().position = initialPosition;
            transform.rotation = Quaternion.Euler(initialRotation);
            isDragging = false;
            UIManager.manager.SendData(new SignalData(GameSignal.END_DRAGGING_CARD));
        }

        public IEnumerator ActivateAura()
        {
            if (card.CanBeUsed() && CanInteract() && card.GetUser().GetComponent<TurnCombat>().IsYourTurn())
            {
                usableAura.enabled = true;
            } 
            else
            {
                usableAura.enabled = false;
            }
            yield return null;
            AnimationQueue.Instance.EndAnimation();
        }


        private bool CanInteract()
        {
            return (!backImage.enabled && !onAnimation);
        }

        #region Getters and setters

        public void SetOnAnimation(bool onAnimation)
        {
            this.onAnimation = onAnimation;
        }

        public bool IsDragging()
        {
            return isDragging;
        }

        public void SetPosition(Vector3 position)
        {
            initialPosition = position;
        }

        public Vector3 GetPosition()
        {
            return initialPosition;
        }

        public Vector3 GetRotation()
        {
            return initialRotation;
        }

        public bool IsCardBack()
        {
            return backImage.enabled;
        }

        public void SetRotation(Vector3 rotation)
        {
            initialRotation = rotation;
        }
        #endregion


        #region Debug

        [Button]
        private void SetFrontCard()
        {
            SetCardBack(!backImage.IsActive());
        }
        #endregion

        public void OnCompleted()
        {
            disposable.Dispose();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(SignalData value)
        {
            if ((value.signal.Equals(GameSignal.START_TURN) || value.signal.Equals(GameSignal.END_TURN) || value.signal.Equals(GameSignal.CARD_DRAWED) || value.signal.Equals(GameSignal.RESOURCE_MODIFY) || value.signal.Equals(GameSignal.CARD_PLAYED_CANCEL))
                    && (value as CombatSignalData).user.Equals(card.GetUser()))
            {
                AnimationQueue.Instance.AddAnimationToQueue(ActivateAura());
            } 
            else if (value.signal.Equals(GameSignal.END_COMBAT))
            {
                disposable.Dispose();
            }
        }
    }

}
