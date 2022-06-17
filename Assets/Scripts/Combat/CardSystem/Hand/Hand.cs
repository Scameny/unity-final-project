using System.Collections;
using System.Collections.Generic;
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
                card.SetVisibility(false);
                return true;
            }
            else
                return false;
        }
        public Card RemoveNextCard()
        {
            if (currentHand.Count > 0)
            {
                Card card = currentHand[currentHand.Count - 1];
                currentHand.Remove(card);
                return card;
            }
            throw new EmptyCardContainerException(GetType().Name);
        }

        virtual public void AddCard(Card card)
        {
            currentHand.Add(card);
            card.SetVisibility(true);
            card.transform.SetParent(transform);
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
            while (currentHand.Count != 0)
            {
                Card card = currentHand[0];
                currentHand.Remove(card);
                Destroy(card.gameObject);
            }
        }
    }

}
