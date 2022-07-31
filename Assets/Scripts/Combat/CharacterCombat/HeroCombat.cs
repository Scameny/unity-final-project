using System;
using UnityEngine;
using Character.Stats;
using System.Collections;
using UI;
using Character.Character;
using CardSystem;
using System.Collections.Generic;

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
            UIManager.manager.ActivateCombatUI(true);
            UIManager.manager.CombatUIInteractable(false);
        }

        override public void TurnPreparationStop()
        {
            base.TurnPreparationStop();
            UIManager.manager.ActivateCombatUI(false);
        }

        protected override void EndTurn()
        {
            StartCoroutine(EndTurnCoroutine());
        }

        private IEnumerator EndTurnCoroutine()
        {
            yield return new WaitUntil(() => queueCoroutine == null);
            base.EndTurn();
            UIManager.manager.CombatUIInteractable(false);
        }

        protected override void StartOfTurn()
        {
            base.StartOfTurn();
            UIManager.manager.CombatUIInteractable(true);
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
