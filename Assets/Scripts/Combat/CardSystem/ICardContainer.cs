using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardSystem
{
    public interface ICardContainer
    {
        public bool RemoveCard(Card card);

        public Card RemoveNextCard();

        public void AddCard(Card card);

        public void CreateCard(GameObject user, Usable cardUse, bool oneUse,GameObject cardPrefab);

        public int GetCurrentCardsNumber();

        public IEnumerable<Card> GetCards();

        public void ClearCards();
    }

}
