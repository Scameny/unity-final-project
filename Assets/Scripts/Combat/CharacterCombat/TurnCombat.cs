using System.Collections;
using UnityEngine;
using Character.Character;
using CardSystem;
using GameManagement;
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
        Deck deck;
        Hand hand;
        CardSystem.Stack stack;
        public GameObject cardPrefab;


        protected DefaultCharacter character;

        protected float turnSpeed;
        protected float turnTime;
        protected bool stopTurn;
        bool yourTurn;


        virtual public void StartCombat()
        {
            InitDeck();
            DrawInitialHand();
            TurnPreparationStart();
            character.ActivePassiveAbilities();
        }

        virtual public void EndCombat()
        {
            StopAllCoroutines();
            character.ResetTemporaryResources();
            character.DisposePassiveAbilities();
            RemoveBuffs();
            ClearCards();
        }


        virtual protected void Update()
        {
        }

        public DefaultCharacter GetCharacter()
        {
            return character;
        }

        #region Card operations
        void InitDeck()
        {
            int count = 0;
            List<SignalData> toRet = new List<SignalData>();
            foreach (var item in character.GetUsableCards())
            {
                deck.CreateCard(gameObject, item, false, cardPrefab);
                toRet.Add(new CombatCardSignalData(GameSignal.CARD_CREATED,gameObject, CombatManager.combatManager.GetCharactersInCombat(), deck.GetCardInIndex(count)));
                count++;
            }
            deck.ShuffleDeck();
            toRet.Add(new CardContainerSignalData(GameSignal.DECK_INITIALIZE, gameObject, deck, deck.GetCards()));
            character.SendSignalData(toRet, true);
        }

        public List<SignalData> DrawCard(int numCards, bool sendUISignal = false)
        {
            List<SignalData> toRet = new List<SignalData>();
            for (int i = 0; i < numCards; i++)
            {
                if (deck.GetCurrentCardsNumber() > 0)
                {
                    Card card = deck.DrawCard();
                    if (hand.GetCurrentCardsNumber() < 10)
                    {
                        hand.AddCard(card);
                        toRet.Add(new CombatCardSignalData(GameSignal.CARD_DRAWED, gameObject, CombatManager.combatManager.GetCharactersInCombat(), card));
                    }
                    else
                    {
                        stack.AddCard(card);
                    }
                }
                else
                {
                    toRet.AddRange(RechargeDeck(sendUISignal));
                }
            }
            character.SendSignalData(toRet, sendUISignal);
            return toRet;
        }

        private List<SignalData> RechargeDeck(bool sendUISignal = false)
        {
            List<SignalData> toRet = new List<SignalData>();
            foreach (var item in stack.RemoveAllCards())
            {
                deck.AddCard(item);
            }
            toRet.Add(new CardContainerSignalData(GameSignal.RECHARGE_DECK, gameObject, deck, deck.GetCards()));
            character.SendSignalData(toRet, sendUISignal);
            return toRet;
        }

        /// <summary>
        /// Send to the stack a card, wherever it is
        /// </summary>
        /// <param name="card">The card that is gonna be  sent to the stack</param>
        public List<SignalData> SendToStack(Card card, bool sendUISignal = false)
        {
            List<SignalData> toRet = new List<SignalData>();
            card.transform.parent.GetComponent<ICardContainer>().RemoveCard(card);
            stack.AddCard(card);
            toRet.Add(new CombatCardSignalData(GameSignal.SEND_TO_STACK, gameObject, CombatManager.combatManager.GetCharactersInCombat(), card));
            character.SendSignalData(toRet, sendUISignal);
            return toRet;
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
            DrawCard(character.GetMaxCardsInHand(), true);
        }

        virtual public void CardUsed(Card card)
        {
            character.SendSignalData(new CombatCardSignalData(GameSignal.CARD_USED, gameObject, CombatManager.combatManager.GetCharactersInCombat(), card), true);
            if (card.IsOneUse())
            {
                card.DestroyCard();
            }
            else
            {
                SendToStack(card, true);
            }
        }

        protected void ClearCards()
        {
            deck.ClearCards();
            stack.ClearCards();
            hand.ClearCards();
        }

        #endregion

        #region Traits operation

        protected void RemoveBuffs()
        {
            character.RemoveCombatBuffs();
        }

        protected void EvaluateBuffs()
        {
            character.ReduceTurnInTemporaryBuffs();
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

        public void TurnPreparationStop()
        {
            StopCoroutine(nameof(TurnPreparation));
        }

        virtual protected void StartOfTurn()
        {
            CombatManager.combatManager.PauseCombat();
            character.SendSignalData(new CombatSignalData(GameSignal.START_TURN, gameObject, CombatManager.combatManager.GetCharactersInCombat()), true);
            EvaluateBuffs();
            DrawCard(1, true);
            yourTurn = true;
            turnTime = 0;
        }

        virtual public void EndOfTurn()
        {
            character.SendSignalData(new CombatSignalData(GameSignal.END_TURN, gameObject, CombatManager.combatManager.GetCharactersInCombat()), true);
            yourTurn = false;
            CombatManager.combatManager.ResumeCombat();
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
                    else if (!CombatManager.combatManager.IsTurnPaused())
                    {
                        StartOfTurn();
                    }
                }
                yield return new WaitForSeconds(GameManager.gm.combatTurnWait);                
            }
            
        }
        #endregion

        public bool IsYourTurn()
        {
            return yourTurn;
        }

        public Deck GetDeck()
        {
            return deck;
        }

        public Hand GetHand()
        {
            return hand;
        }

        public void InitializeCardCotainers(Deck deck, Hand hand, CardSystem.Stack stack)
        {
            this.deck = deck;
            this.hand = hand;
            this.stack = stack;
        }
    }
}