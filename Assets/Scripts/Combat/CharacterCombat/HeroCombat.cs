using UnityEngine;
using Character.Stats;
using System.Collections;
using UI;
using Character.Character;
using CardSystem;
using System.Collections.Generic;
using GameManagement;

namespace Combat 
{
    public class HeroCombat : TurnCombat
    {
        Queue<Card> cardsQueue = new Queue<Card>();
        Card nextCardToPlay;
        Coroutine queueCoroutine;

        private void Awake()
        {
            character = GetComponent<Hero>();
        }
        
        private void Start()
        {
            turnSpeed = character.GetStatistic(StatType.Agility);
        }

        public override void StartCombat()
        {
            base.StartCombat();
        }

        public override void EndCombat()
        {
            character.SendSignalData(new CombatSignalData(GameSignal.END_COMBAT, gameObject, CombatManager.combatManager.GetCharactersInCombat()));
            base.EndCombat();
        }

        override protected void Update()
        {
            if (character.IsDead())
            {
                CombatManager.combatManager.HeroDeath();
            }
            base.Update();
        }

        public override void TurnPreparationStart()
        {
            base.TurnPreparationStart();
            character.SendSignalData(new CombatSignalData(GameSignal.TURN_PREPARATION_START, gameObject, CombatManager.combatManager.GetCharactersInCombat()));
        }


        public override void EndTurn()
        {
            StartCoroutine(EndTurnCoroutine());
        }

        private IEnumerator EndTurnCoroutine()
        {
            yield return new WaitUntil(() => queueCoroutine == null);
            base.EndTurn();
        }

        public override void CardUsed(Card card)
        {
            nextCardToPlay = null;
            base.CardUsed(card);
        }

        public void AddCardToQueue(Card card)
        {
            cardsQueue.Enqueue(card);
            if (queueCoroutine == null)
                queueCoroutine = StartCoroutine(UseCardsInQueue());
            card.SetVisibility(false);
        }

        override public void CancelCardUse(Card card)
        {
            nextCardToPlay = null;
            card.SetVisibility(true);
        }

        private IEnumerator UseCardsInQueue()
        {
            while (cardsQueue.Count > 0)
            {
                yield return new WaitUntil(() => nextCardToPlay == null);
                nextCardToPlay = cardsQueue.Peek();
                try
                {
                    nextCardToPlay.UseCard();
                    cardsQueue.Dequeue();
                }
                catch
                {
                    cardsQueue.Dequeue().SetVisibility(true);
                    nextCardToPlay = null;
                }
            }
            queueCoroutine = null;
        }
    }

}
