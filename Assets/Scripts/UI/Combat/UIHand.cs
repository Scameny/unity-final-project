using Animations;
using CardSystem;
using DG.Tweening;
using GameManagement;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Combat
{
    /// <summary>
    /// Class for all the view operations of the player hand.
    /// </summary>
    public class UIHand : MonoBehaviour, IObserver<SignalData>
    {
        [SerializeField] int overlappingPerCard, startingOverlap, quantityCardsToStartOverlapping, maxZCardRotation, maxYCardPosition;
        [SerializeField] float secondsBetweenDrawingCards;

        IDisposable disposable;
        GameObject character, cardPrefab;

        private void Start()
        {
            disposable = UIManager.manager.Subscribe(this);
            character = GameObject.FindGameObjectWithTag("Player");
            cardPrefab = Resources.Load<GameObject>("UI/UICombatCard");
        }

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
            if (value.signal.Equals(GameSignal.CARD_DRAWED) && character.Equals((value as CombatCardSignalData).user))
            {
                CombatCardSignalData data = value as CombatCardSignalData;
                AnimationQueue.Instance.AddAnimationToQueue(DrawCardAnimation(data.card));
            }
            else if ((value.signal.Equals(GameSignal.CARD_PLAYED) || value.signal.Equals(GameSignal.CARD_PLAYED_CANCEL)) && character.Equals((value as CombatCardSignalData).user))
            {
                FitCards();
            }
        }


        private IEnumerator DrawCardAnimation(Card card)
        {
            UICombatCard uiCard = card.GetComponent<UICombatCard>();
            uiCard.SetOnAnimation(true);
            FitCards();
            uiCard.SetCardBack(false);
            card.transform.DOMove(card.GetComponent<UICombatCard>().GetPosition(), secondsBetweenDrawingCards).OnComplete(() =>
            {
                card.transform.SetParent(transform);
                card.transform.rotation = Quaternion.Euler(card.GetComponent<UICombatCard>().GetRotation());
                uiCard.SetOnAnimation(false);
                AnimationQueue.Instance.EndAnimation();
                return;
            });
            yield return null;
        }

        private void FitCards()
        {
            Hand hand = GetComponent<Hand>();
            int numberOfCards = hand.GetCurrentCardsNumber();
            int count = 0;
            int overlap = startingOverlap + (numberOfCards - quantityCardsToStartOverlapping) * overlappingPerCard;
            Func<int, float> coeficientFunc;
            Vector3 position, rotation, initialPosition;
            if (numberOfCards % 2 == 1)
            {
                coeficientFunc = (int x) =>
                {
                    float middleValue = Mathf.Floor(numberOfCards / 2);
                    if (middleValue == 0)
                    {
                        return 0.0f;
                    }
                    else
                    {
                        return (middleValue - x) / middleValue;
                    }
                };
                initialPosition = transform.position;
            }
            else
            {
                coeficientFunc = (int x) =>
                {
                    float middleValue = (numberOfCards - 1) / 2.0f;
                    return (middleValue - x) / middleValue;
                };
                initialPosition = transform.position + new Vector3(cardPrefab.GetComponent<RectTransform>().rect.width / 2, 0, 0);
            }


            foreach (var item in hand.GetCards())
            {
                rotation = new Vector3(0, 0, maxZCardRotation * coeficientFunc(count));
                position = initialPosition + new Vector3((count - numberOfCards / 2) * (item.GetComponent<RectTransform>().rect.width - overlap), (1 - Math.Abs(coeficientFunc(count))) * maxYCardPosition, 0);
                item.GetComponent<UICombatCard>().SetPosition(position);
                item.GetComponent<UICombatCard>().SetRotation(rotation);
                if (!item.GetComponent<UICombatCard>().IsDragging() && !item.GetComponent<UICombatCard>().IsCardBack())
                {
                    item.transform.position = position;
                    item.transform.rotation = Quaternion.Euler(rotation);
                }
                count += 1;
            }
        }

        [Button]
        private void FitCardsDebug()
        {
            cardPrefab = Resources.Load<GameObject>("UI/UICombatCard");
        }
    }

}