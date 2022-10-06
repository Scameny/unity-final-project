using CardSystem;
using Character.Character;
using Combat;
using GameManagement;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Strategies.PassiveEffectStrategies
{ 
    public class EvolveCardsPassiveEffect : PassiveEffectStrategy
    {
        [TableList]
        [SerializeField] List<CardSwap> cardSwaps = new List<CardSwap>();
        protected override List<SignalData> EffectAction(CombatSignalData passiveData)
        {
            List<Card> cards = new List<Card>();
            cards.AddRange(passiveData.user.GetComponent<TurnCombat>().GetDeck().GetCards());
            cards.AddRange(passiveData.user.GetComponent<TurnCombat>().GetHand().GetCards());
            cards.AddRange(passiveData.user.GetComponent<TurnCombat>().GetStack().GetCards());
            List<SignalData> toRet = new List<SignalData>();
            foreach (var cardSwap in cardSwaps)
            {
                foreach (var card in cards)
                {
                    if (card.GetUsable().GetName().Equals(cardSwap.GetBaseCard().GetName()))
                    {
                        card.ReinitializeCard(cardSwap.GetImprovedCard());
                        toRet.Add(new EvolveCardSignalData(GameSignal.CARD_EVOLVED, card, cardSwap.GetImprovedCard()));
                    }
                }
            }
            return toRet;
        }
    }
}