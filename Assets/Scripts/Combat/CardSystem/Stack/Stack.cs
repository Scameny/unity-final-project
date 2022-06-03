using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardSystem
{
    public class Stack : MonoBehaviour, ICardContainer
    {
        [SerializeField] protected List<Card> currentStack = new List<Card>();

        virtual public void AddCard(Card card)
        {
            card.transform.SetParent(transform);
            currentStack.Add(card);
        }

        public void ClearTemporaryCards()
        {
            throw new System.NotImplementedException();
        }

        virtual public void CreateCard(GameObject user, Usable cardUse, bool temporary, bool oneUse, GameObject cardPrefab)
        {
            throw new System.NotImplementedException();
        }

        public int GetCurrentCardsNumber()
        {
            return currentStack.Count;
        }

        virtual public Card RemoveCard(Card card)
        {
            if (currentStack.Remove(card))
                return card;
            else
                throw new NotValidOperationException("", GetType().Name);
        }

        public Card RemoveNextCard()
        {
            if (GetCurrentCardsNumber() > 0)
            {
                Card card = currentStack[0];
                currentStack.RemoveAt(0);
                return card;
            }
            else
            {
                throw new NotValidOperationException("", GetType().Name);
            }
        }
    }
}
