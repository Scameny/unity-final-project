using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        public Card RemoveNextCard()
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

        virtual public Card DrawCard()
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
            while(currentDeck.Count != 0)
            {
                Card card = currentDeck[0];
                currentDeck.Remove(card);
                Destroy(card.gameObject);
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
