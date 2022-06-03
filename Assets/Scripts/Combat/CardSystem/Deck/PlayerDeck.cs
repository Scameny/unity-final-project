using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardSystem
{
    public class PlayerDeck : Deck
    {
        override public void RechargeDeck(List<Card> cards)
        {
            // Animation
            if (currentDeck.Count == 0)
            {
                foreach (var card in cards)
                {
                    AddCard(card);
                }
                currentDeck.Shuffle();
            }
            else
            {
                throw new NotValidOperationException("", GetType().Name);
            }

        }

        [Button]
        public void DrawCardDebug()
        {
            DrawCard();
        }
    }

}
