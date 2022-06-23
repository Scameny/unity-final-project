using UnityEngine;
using System.Linq;
using Character.Stats;
using Character.Character;
using CardSystem;

namespace Combat
{
    public class EnemyCombat : TurnCombat
    {
        // Data of the enemy class , level, etc..

        private void Awake()
        {
            character = GetComponent<Npc>();
        }

        private void Start()
        {
            turnSpeed = character.GetStatistic(StatType.Agility);
        }

        // Update is called once per frame
        override protected void Update()
        {
            if (character.IsDead())
            {
                CombatManager.combatManager.EnemyDeath(this);
            }
            base.Update();
        }

        protected override void StartOfTurn()
        {
            base.StartOfTurn();
            EnemyIA();
        }

        #region Cards operations

        protected override void InitDeck()
        {
            base.InitDeck();
        }

        private void EnemyIA()
        {
            while(hand.GetCurrentCardsNumber() > 0)
            {            
                try
                {
                    UseCard(hand.GetNextCard());
                }
                catch (NotEnoughResourceException e)
                {
                    break;
                }
            }
            EndTurn();
        }

        private Card SearchCardOfType(CardEffectType effectType)
        {
            foreach (var item in hand.GetCards())
            {
                if (item.GetCardEffect().Any(e => e.Equals(effectType)))
                {
                    if (item.GetResourceCost().All(r => character.GetResourceType().Contains(r.resourceType)))
                        return item;
                }
                    
            }
            return null;
        }

        private void UseCard(Card card)
        {
            hand.RemoveCard(card);
            card.UseCard();
        }
        #endregion
    }
}
