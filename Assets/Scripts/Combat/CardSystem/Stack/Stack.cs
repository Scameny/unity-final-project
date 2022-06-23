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
        virtual public bool RemoveCard(Card card)
        {
            return currentStack.Remove(card);
        }

        public Card GetNextCard()
        {
            if (currentStack.Count > 0)
            {
                Card card = currentStack[currentStack.Count];
                currentStack.Remove(card);
                return card;
            }
            return null;
        }

        virtual public void CreateCard(GameObject user, Usable cardUse, bool oneUse, GameObject cardPrefab)
        {
            GameObject cardGameObject = Instantiate(cardPrefab, transform);
            Card card = cardGameObject.GetComponent<Card>();
            card.InitializeCard(cardUse, user, oneUse);
            card.SetVisibility(false);
        }

        public int GetCurrentCardsNumber()
        {
            return currentStack.Count;
        }

        public void ClearCards()
        {
            while(currentStack.Count != 0)
            {
                Card card = currentStack[0];
                currentStack.Remove(card);
                Destroy(card.gameObject);
            }
        }

        public IEnumerable<Card> GetCards()
        {
            return currentStack;
        }

        public IEnumerable<Card> RemoveAllCards()
        {
            while(currentStack.Count != 0)
            {
                Card card = currentStack[0];
                currentStack.Remove(card);
                yield return card;
            }
        }
    }
}
