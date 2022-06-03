using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardSystem
{
    public class Hand : MonoBehaviour, ICardContainer
    {
        [SerializeField] protected List<Card> currentHand = new List<Card>();

        virtual public void AddCard(Card card)
        {
            currentHand.Add(card);
            card.SetVisibility(true);
            card.transform.SetParent(transform);
        }

        public void CreateCard(GameObject user, IUsable cardUse, bool temporary, GameObject cardPrefab)
        {
            throw new System.NotImplementedException();
        }

        public int GetCurrentCardsNumber()
        {
            return currentHand.Count;
        }

        virtual public Card RemoveCard(Card card)
        {
            if (currentHand.Remove(card))
            {
                card.SetVisibility(false);
                return card;
            }
            else
                throw new NotValidOperationException("", GetType().Name);
        }

        public Card GetNextCard()
        {
            Card card = currentHand[0];
            return card;
        }

        virtual public bool UseCard(Card card)
        {
            throw new System.NotImplementedException();
        }

        public Card RemoveNextCard()
        {
            throw new System.NotImplementedException();
        }
    }

}
