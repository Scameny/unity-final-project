using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using Character.Character;
using CardSystem;
using System.Collections.Generic;

namespace Combat
{
    /// <summary>
    /// Class that should inherit every script that manage combat.
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
        
        
        protected bool prepared = false;
        protected float turnSpeed;
        protected float turnTime;
        protected bool stopTurn;



        private void OnEnable()
        {
            TurnPreparationStart();
            EnableHealthBar(true);
            InitDeck();
            DrawInitialHand();
        }

        private void OnDisable()
        {
            EnableHealthBar(false);
            TurnPreparationStop();
            RefillPermanentDeck();
        }

        public DefaultCharacter GetCharacter()
        {
            return character;
        }

        #region Health operations
        public void TakeDamage(float damage, DamageType type)
        {
            character.TakeDamage(damage, type);
        }
        #endregion

        #region Card operations
        virtual protected void InitDeck()
        {
            deck.InitDeck();
        }

        public void DrawCard()
        {
            if (deck.GetCurrentCardsNumber() != 0)
            {
                Card card = deck.DrawCard();
                hand.AddCard(card);
            }
            else if (hand.GetCurrentCardsNumber() == 0)
            {
                RechargeDeck();
            }
        }
        private void RechargeDeck()
        {
            List<Card> cards = new List<Card>();
            while (stack.GetCurrentCardsNumber() > 0)
            {
                cards.Add(stack.RemoveNextCard());
            }
            deck.RechargeDeck(cards);
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
        public void AddNewCardToDeck(IUsable usable, bool temporary)
        {
            deck.CreateCard(gameObject, usable, temporary, cardPrefab);
        }

        /// <summary>
        /// Send all the permanent cards to the permanent deck.
        /// </summary>
        public void RefillPermanentDeck()
        {
            deck.RefillPermanentDeck();
        }

        protected void DrawInitialHand()
        {
            for (int i = 0; i < character.GetMaxCardsInHand(); i++)
            {
                Debug.Log("Robamos carta");
                DrawCard();
            }
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