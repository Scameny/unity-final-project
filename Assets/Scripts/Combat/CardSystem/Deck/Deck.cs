using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardSystem
{
    public abstract class Deck : MonoBehaviour, ICardContainer
    {
        [SerializeField] protected List<Card> currentDeck = new List<Card>();
        [SerializeField] protected List<Card> permanentDeck = new List<Card>();


        virtual public void RechargeDeck(List<Card> cards)
        {
            throw new System.NotImplementedException();
        }

        public void AddPermanentDeckToCurrentDeck()
        {
            foreach (var card in permanentDeck)
            {
                AddCard(card);
            }
        }

        public void ShuffleCurrentDeck()
        {
            currentDeck.Shuffle();
        }

        public void RefillPermanentDeck()
        {
            foreach (var item in permanentDeck)
            {
                item.transform.SetParent(transform);
                item.SetVisibility(false);
            }
        }

        virtual public Card DrawCard()
        {
            Card card = currentDeck[currentDeck.Count - 1];
            currentDeck.RemoveAt(currentDeck.Count - 1);
            return card;
        }

        virtual public void AddCard(Card card)
        {
            currentDeck.Add(card);
            card.transform.SetParent(transform);
        }

        public Card RemoveCard(Card card)
        {
            throw new System.NotImplementedException();
        }

        virtual public void CreateCard(GameObject user, Usable cardUse, bool temporary, bool oneUse, GameObject cardPrefab)
        {
            GameObject cardGameObject = Instantiate(cardPrefab, transform);
            Card card = cardGameObject.GetComponent<Card>();
            card.InitializeCard(cardUse, user, temporary, oneUse);
            card.SetVisibility(false);
            if (temporary)
                currentDeck.Add(card);
            else
                permanentDeck.Add(card);
        }

        public int GetCurrentCardsNumber()
        {
            return currentDeck.Count;
        }

        public Card RemoveNextCard()
        {
            throw new System.NotImplementedException();
        }

        public void ClearTemporaryCards()
        {
            foreach (var item in currentDeck)
            {
                Destroy(item);
            }
        }
    }
    static class ShuffleClass
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }

}
