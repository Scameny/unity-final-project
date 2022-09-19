using System.Collections.Generic;
using UnityEngine;

namespace CardSystem
{
    public interface ICardContainer
    {
        public bool RemoveCard(Card card);

        public Card GetNextCard();

        public void AddCard(Card card);

        public void AddCard(Card card, int index);

        public void CreateCard(GameObject user, Usable cardUse, bool oneUse,GameObject cardPrefab);

        public int GetCurrentCardsNumber();

        public IEnumerable<Card> GetCards();

        public IEnumerable<Card> RemoveAllCards();

        public void ClearCards();

        public int GetIndex(Card card);
    }

}
