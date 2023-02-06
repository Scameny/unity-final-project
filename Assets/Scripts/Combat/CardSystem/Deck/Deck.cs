using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

namespace CardSystem
{
    public abstract class Deck : MonoBehaviour, ICardContainer
    {
        [SerializeField] protected List<Card> currentDeck = new List<Card>();

        virtual public void AddCard(Card card)
        {
            card.transform.SetParent(transform);
            currentDeck.Add(card);
        }

        public bool RemoveCard(Card card)
        {
            return currentDeck.Remove(card);
        }
        public Card GetNextCard()
        {
            if (currentDeck.Count > 0)
            {
                Card card = currentDeck[currentDeck.Count];
                currentDeck.Remove(card);
                return card;
            }
            throw new EmptyCardContainerException(GetType().Name);
        }

        virtual public void CreateCard(GameObject user, Usable cardUse, bool oneUse, GameObject cardPrefab)
        {
            GameObject cardGameObject = Instantiate(cardPrefab, transform);
            Card card = cardGameObject.GetComponent<Card>();
            card.InitializeCard(cardUse, user, oneUse);
            card.SetVisibility(false);
            currentDeck.Add(card);
        }

        public int GetCurrentCardsNumber()
        {
            return currentDeck.Count;
        }

        public void ShuffleDeck()
        {
            currentDeck.Shuffle();
        }

        public Card DrawCard()
        {
            if (currentDeck.Count > 0)
            {
                Card card = currentDeck[currentDeck.Count - 1];
                currentDeck.Remove(card);
                return card;
            }
            throw new EmptyCardContainerException(GetType().Name);
        }

        public IEnumerable<Card> GetCards()
        {
            foreach (var item in currentDeck)
            {
                yield return item;
            }
        }

        public void ClearCards()
        {
            foreach (var item in currentDeck.ToList())
            {
                currentDeck.Remove(item);
                item.DestroyCard();
            }
        }

        public IEnumerable<Card> RemoveAllCards()
        {
            throw new System.NotImplementedException();
        }

        public void AddCard(Card card, int index)
        {
            throw new NotImplementedException();
        }

        public int GetIndex(Card card)
        {
            throw new NotImplementedException();
        }

        public Card GetCardInIndex(int index)
        {
            return currentDeck[index];
        }
    }

}
