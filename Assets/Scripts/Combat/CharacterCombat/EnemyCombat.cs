using System.Linq;
using Character.Stats;
using Character.Character;
using CardSystem;
using UI;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace Combat
{
    public class EnemyCombat : TurnCombat
    {

        EnemyUI enemyUI;
        GameObject player;

        Queue<Card> cardsQueue = new Queue<Card>();
        Card nextCardToPlay;

        private void Awake()
        {
            character = GetComponent<Npc>();
            enemyUI = GetComponentInChildren<EnemyUI>();
        }

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
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
            StartCoroutine(UseCardsInQueue());
        }

        #region IA operations
        private void EnemyIA()
        {
            IA_KillPlayerIfPossible();
            IA_UseBuffsAndDebuffs();
            IA_KillPlayerIfPossible();
            IA_HealIfNeeded();
            IA_CardsDraw();
            IA_KillPlayerIfPossible();
            IA_UseDamageCards();
            EndTurn();
        }

        private void IA_KillPlayerIfPossible()
        {
            int totalDamage = 0;
            bool notEnoughResources = false;
            int playerHealth = player.GetComponent<DefaultCharacter>().GetCurrentHealth();
            List<Card> cardsToPlay = new List<Card>();
            Dictionary<ResourceType, int> resources = new Dictionary<ResourceType, int>();
            foreach (var item in SearchCardsOfType(CardEffectType.Damage))
            {
                notEnoughResources = false;
                foreach (var resourceCost in item.GetResourceCost())
                {
                    if (!resources.ContainsKey(resourceCost.resourceType)) 
                    {
                        resources.Add(resourceCost.resourceType, character.GetCurrentResource(resourceCost.resourceType));
                    }
                    if (resources[resourceCost.resourceType] - resourceCost.amount < 0)
                    {
                        notEnoughResources = true;
                        foreach (var resourceCard in SearchResourceCards(resourceCost.resourceType))
                        {
                            resources[resourceCost.resourceType] += resourceCard.GetUsable().GetSimulatedResourceGained(gameObject, resourceCost.resourceType);
                            cardsToPlay.Add(resourceCard);
                            if (resources[resourceCost.resourceType] - resourceCost.amount > 0)
                            {
                                notEnoughResources = false;
                                break;
                            }
                        }
                    }
                }
                if (!notEnoughResources)
                {
                    cardsToPlay.Add(item);
                    totalDamage += item.GetUsable().GetSimulatedDamage(gameObject);
                    if (totalDamage >= playerHealth)
                        break;
                }
            }
            if (totalDamage >= playerHealth)
            {
                foreach (var item in cardsToPlay)
                {
                    AddCardToQueue(item);
                }
            }

        }

        private void IA_UseBuffsAndDebuffs()
        {
            List<Card> cards = SearchCardsOfType(CardEffectType.Buff);
            cards.AddRange(SearchCardsOfType(CardEffectType.Debuff));
            while (cards.Count > 0)
            {
                AddCardToQueue(cards[0]);
                cards.RemoveAt(0);
            }
        }

        private void IA_HealIfNeeded()
        {
            while((character.GetCurrentHealth() / character.GetMaxHealth()) < (character as Npc).GetIAPercentageForHealing())
            {
                Card card = SearchCardOfType(CardEffectType.Heal);
                if (card != null)
                    AddCardToQueue(card);
                else
                    break;
            }
        }

        private void IA_CardsDraw()
        {
            List<Card> cards = SearchCardsOfType(CardEffectType.DrawCards);
            while (cards.Count > 0)
            {
                AddCardToQueue(cards[0]);
                cards.RemoveAt(0);
            }
        }

        private void IA_UseDamageCards()
        {
            List<Card> cards = SearchCardsOfType(CardEffectType.Damage);
            while (cards.Count > 0)
            {
                AddCardToQueue(cards[0]);
                cards.RemoveAt(0);
            }
        }

        #endregion

        #region Cards operations

        private IEnumerable<Card> SearchResourceCards(ResourceType resource)
        {
            foreach (var item in SearchCardsOfType(CardEffectType.ResourceGain))
            {
                foreach (var resourceGained in item.GetUsable().GetResourceGained())
                {
                    if (resource.Equals(resource))
                    {
                        yield return item;
                        break;
                    }
                }
            }
        }

        private Card SearchResourceCard(ResourceType resource)
        {
            foreach (var item in SearchCardsOfType(CardEffectType.ResourceGain))
            {
                foreach (var resourceGained in item.GetUsable().GetResourceGained())
                {
                    if (resource.Equals(resource))
                    {
                        return item;
                    }
                }
            }
            return null;
        }

        protected override void InitDeck()
        {
            base.InitDeck();
        }

        private Card SearchCardOfType(CardEffectType effectType)
        {
            Card cardSpecificEffect = null;
            Card cardWithMoreThanOneEffect = null;

            foreach (var item in hand.GetCards())
            {
                if (item.GetCardEffect().Any(e => e.Equals(effectType)))
                {
                    if (item.GetCardEffect().Count() == 1)
                        cardSpecificEffect = item;
                    else
                        cardWithMoreThanOneEffect = item;   
                }   
            }
            return null;
        }

        private List<Card> SearchCardsOfType(CardEffectType effectType)
        {
            List<Card> cardsToRet = new List<Card>();
            foreach (var item in hand.GetCards())
            {
                if (item.GetCardEffect().Any(e => e.Equals(effectType)))
                {
                    cardsToRet.Add(item);
                }
            }
            return cardsToRet;
        }

        public override void CardUsed(Card card)
        {
            enemyUI.ShowCard(card);
            nextCardToPlay = null;
            base.CardUsed(card);
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
                catch(NotEnoughResourceException e)
                {
                    nextCardToPlay = SearchResourceCard(e.resource);
                    if (nextCardToPlay != null)
                        nextCardToPlay.UseCard();
                    else
                        cardsQueue.Dequeue();

                }
            }
        }

        private void AddCardToQueue(Card card)
        {
            cardsQueue.Enqueue(card);
        }
        #endregion
    }
}
