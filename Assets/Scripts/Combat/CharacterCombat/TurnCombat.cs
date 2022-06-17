using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using Character.Character;
using CardSystem;
using Abilities.Passive;
using System;
using System.Collections.Generic;

namespace Combat
{
    /// <summary>
    /// Class that should inherit every script that manage combat.
    /// It's in charge of Card system in combat and turn base system
    /// When enabled, it starts the turn based system.
    /// </summary>
    public class TurnCombat : MonoBehaviour
    {
        [Header("Card management")]
        [SerializeField] protected Deck deck;
        [SerializeField] protected Hand hand;
        [SerializeField] protected CardSystem.Stack stack;
        public GameObject cardPrefab;

        [Header("UI Elements")]
        public Slider healthBar;
        public GameObject selector;

        [HideInInspector]
        protected DefaultCharacter character = null;


        protected PassiveManager passiveManager = new PassiveManager();
        protected bool prepared = false;
        protected float turnSpeed;
        protected float turnTime;
        protected bool stopTurn;



        private void OnEnable()
        {
            EnableHealthBar(true);
            InitDeck();
            DrawInitialHand();
            TurnPreparationStart();
            ActivePassiveAbilities();
        }

        private void OnDisable()
        {
            EnableHealthBar(false);
            TurnPreparationStop();
            ClearCards();
        }

        virtual protected void Update()
        {
            EvaluateTraits();
            UpdateHealthBar();
        }

        public DefaultCharacter GetCharacter()
        {
            return character;
        }

        #region Passive operations

        protected void ActivePassiveAbilities()
        {
            foreach (var item in character.GetCurrentPassiveAbilities())
            {
                IDisposable disposable = passiveManager.Subscribe(item);
                item.SetDisposable(disposable);
            }
        }

        protected void SendPassiveData(PassiveSignal signal)
        {
            passiveManager.SendData(signal, gameObject, CombatManager.combatManager.GetCharactersInCombat());
        }
       
        #endregion

        #region Health operations
        public void TakeDamage(float damage, DamageType type)
        {
            character.TakeDamage(damage, type);
        }
        #endregion

        #region Card operations
        virtual protected void InitDeck()
        {
            foreach (var item in character.GetUsableCards())
            {
                deck.CreateCard(gameObject, item, false, cardPrefab);
            }
            deck.ShuffleDeck();
        }

        public void DrawCard(int numCards)
        {
            for (int i = 0; i < numCards; i++)
            {
                try
                {
                    Card card = deck.DrawCard();
                    if (hand.GetCurrentCardsNumber() < 10)
                        hand.AddCard(card);
                    else
                        stack.AddCard(card);
                }
                catch (EmptyCardContainerException e)
                {
                    RechargeDeck();
                }
            }
        }

        private void RechargeDeck()
        {
            foreach (var card in stack.GetCards())
            {
                stack.RemoveCard(card);
                deck.AddCard(card);
            }
            TurnPreparationResume();
        }

        /// <summary>
        /// Send to the stack a card, wherever it is
        /// </summary>
        /// <param name="card">The card that is gonna be  sent to the stack</param>
        public void SendToStack(Card card)
        {
            card.transform.parent.GetComponent<ICardContainer>().RemoveCard(card);
            stack.AddCard(card);
        }

        /// <summary>
        /// Create a new card for the deck. If temporary is true, only for the current deck otherwise for both current and permanent deck
        /// </summary>
        public void AddNewCardToDeck(Usable usable, bool oneUse)
        {
            deck.CreateCard(gameObject, usable, oneUse, cardPrefab);
        }

        protected void DrawInitialHand()
        {
            DrawCard(character.GetMaxCardsInHand());
        }

        protected void ClearCards()
        {
            deck.ClearCards();
            stack.ClearCards();
            hand.ClearCards();
        }

        #endregion

        #region Traits operation
        protected void EvaluateTraits()
        {
            character.ReduceTurnInTemporaryTraits();
        }
        #endregion

        #region Turn management
        public void TurnPreparationStart()
        {
            turnTime = 0.0f;
            stopTurn = false;
            StartCoroutine(nameof(TurnPreparation));
        }

        public virtual void TurnPreparationResume()
        {
            stopTurn = false;
        }

        public void TurnPreparationPause()
        {
            stopTurn = true;
        }

        public void TurnPreparationStop()
        {
            StopCoroutine(nameof(TurnPreparation));
        }

        protected IEnumerator TurnPreparation()
        {
            turnTime = 0;
            while (true)
            {
                if (!stopTurn)
                {
                    if (turnTime < 100.0)
                    {
                        turnTime += turnSpeed;
                    }
                    else
                    {
                        prepared = true;
                        stopTurn = true;
                        turnTime = 0;
                    }
                }
                yield return new WaitForSeconds(0.05f);                
            }
            
        }
        #endregion

        #region UI Management
        protected void EnableHealthBar(bool enable)
        {
            healthBar.gameObject.SetActive(true);
        }


        protected void UpdateHealthBar()
        {
            healthBar.maxValue = character.GetMaxHealth();
            healthBar.value = character.GetCurrentHealth();
        }
        #endregion

    }

}