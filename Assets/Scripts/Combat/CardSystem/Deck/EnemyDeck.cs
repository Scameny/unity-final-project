using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardSystem
{
    public class EnemyDeck : Deck
    {
        override public void RechargeDeck(List<Card> cards)
        {
            // Animation
            currentDeck.Clear();
            currentDeck.AddRange(cards);
            currentDeck.Shuffle();
        }
    }

}
