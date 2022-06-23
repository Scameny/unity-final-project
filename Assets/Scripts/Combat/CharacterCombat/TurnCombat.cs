using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using Character.Character;
using CardSystem;
using Abilities.Passive;
using System;

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


        [HideInInspector]
        protected DefaultCharacter character = null;

        protected float turnSpeed;
        protected float turnTime;
        protected bool stopTurn;



        private void OnEnable()
        {
            InitDeck();
            DrawInitialHand();
            TurnPreparationStart();
            ActivePassiveAbilities();
        }

        private void OnDisable()
        {
            TurnPreparationStop();
            ClearCards();
        }

        virtual protected void Update()
        {
            EvaluateTraits();
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
                IDisposable disposable = character.passiveManager.Subscribe(item);
                item.SetDisposable(disposable);
            }
        }

        protected void SendPassiveData(PassiveSignal signal)
        {
            character.passiveManager.SendData(signal, gameObject, CombatManager.combatManager.GetCharactersInCombat());
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
                    character.passiveManager.SendData(PassiveSignal.CardDrawed, gameObject, CombatManager.combatManager.GetCharactersInCombat());
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
            foreach (var item in stack.RemoveAllCards())
            {
                deck.AddCard(item);
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
        virtual public void TurnPreparationStart()
        {
            turnTime = 0.0f;
            stopTurn = false;
            StartCoroutine(nameof(TurnPreparation));
        }

        public void TurnPreparationResume()
        {
            stopTurn = false;
        }

        public void TurnPreparationPause()
        {
            stopTurn = true;
        }

        virtual public void TurnPreparationStop()
        {
            StopCoroutine(nameof(TurnPreparation));
        }

        virtual protected void StartOfTurn()
        {
            DrawCard(1);
            character.passiveManager.SendData(PassiveSignal.StartOfTurn, gameObject, CombatManager.combatManager.GetCharactersInCombat());
            stopTurn = true;
            turnTime = 0;
        }

        virtual protected void EndTurn()
        {
            character.passiveManager.SendData(PassiveSignal.EndOfTurn, gameObject, CombatManager.combatManager.GetCharactersInCombat());
            TurnPreparationResume();
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
                        StartOfTurn();
                    }
                }
                yield return new WaitForSeconds(0.05f);                
            }
            
        }
        #endregion
    }
}