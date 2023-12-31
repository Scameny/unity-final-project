using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CardSystem
{
    public class Hand : MonoBehaviour, ICardContainer
    {
        [SerializeField] protected List<Card> currentHand = new List<Card>();

        virtual public bool RemoveCard(Card card)
        {
            if (currentHand.Remove(card))
            {
                return true;
            }
            else
                return false;
        }
        public Card GetNextCard()
        {
            if (currentHand.Count > 0)
            {
                Card card = currentHand[currentHand.Count - 1];
                return card;
            }
            throw new EmptyCardContainerException(GetType().Name);
        }

        public void AddCard(Card card)
        {
            currentHand.Add(card);
        }

        public void CreateCard(GameObject user, Usable cardUse, bool oneUse, GameObject cardPrefab)
        {
            GameObject cardGameObject = Instantiate(cardPrefab, transform);
            Card card = cardGameObject.GetComponent<Card>();
            card.InitializeCard(cardUse, user, oneUse);
            card.SetVisibility(true);
        }

        public int GetCurrentCardsNumber()
        {
            return currentHand.Count;
        }

        public IEnumerable<Card> GetCards()
        {
            foreach (var item in currentHand)
            {
                yield return item;
            }
        }

        public void ClearCards()
        {
            foreach (var item in currentHand.ToList())
            {
                currentHand.Remove(item);
                item.DestroyCard();
            }
        }

        public IEnumerable<Card> RemoveAllCards()
        {
            throw new System.NotImplementedException();
        }

        public void AddCard(Card card, int index)
        {
            currentHand.Insert(index, card);
        }

        public int GetIndex(Card card)
        {
            return currentHand.IndexOf(card);
        }
    }

}
