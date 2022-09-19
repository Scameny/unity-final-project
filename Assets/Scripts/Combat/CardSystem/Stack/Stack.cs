using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CardSystem
{
    public class Stack : MonoBehaviour, ICardContainer
    {
        [SerializeField] protected List<Card> currentStack = new List<Card>();

        virtual public void AddCard(Card card)
        {
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
            foreach (var item in currentStack.ToList())
            {
                currentStack.Remove(item);
                item.DestroyCard();
            }
        }

        public IEnumerable<Card> GetCards()
        {
            return currentStack;
        }

        public IEnumerable<Card> RemoveAllCards()
        {
            foreach (var item in currentStack.ToList())
            {
                currentStack.Remove(item);
                yield return item;
            }
        }

        public void AddCard(Card card, int index)
        {
            throw new System.NotImplementedException();
        }

        public int GetIndex(Card card)
        {
            throw new System.NotImplementedException();
        }
    }
}
