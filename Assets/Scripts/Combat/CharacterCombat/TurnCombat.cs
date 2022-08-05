using System.Collections;
using UnityEngine;
using Character.Character;
using CardSystem;
using GameManagement;

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


        protected DefaultCharacter character;

        protected float turnSpeed;
        protected float turnTime;
        protected bool stopTurn;



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
            character.DisposePassiveAbilities();
            RemoveTemporaryTraits();
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
                    character.SendSignalData(new CombatCardSignalData(GameSignal.CARD_DRAWED, gameObject, CombatManager.combatManager.GetCharactersInCombat(), card));
                    if (hand.GetCurrentCardsNumber() < 10)
                        hand.AddCard(card);
                    else
                        stack.AddCard(card);
                }
                catch (EmptyCardContainerException)
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

        virtual public void CardUsed(Card card)
        {
            character.SendSignalData(new CombatCardSignalData(GameSignal.CARD_PLAYED, gameObject, CombatManager.combatManager.GetCharactersInCombat(), card));
            if (card.IsOneUse())
            {
                Destroy(gameObject);
            }
            else
            {
                SendToStack(card);
            }
        }

        virtual public void CancelCardUse(Card card)
        {

        }

        protected void ClearCards()
        {
            deck.ClearCards();
            stack.ClearCards();
            hand.ClearCards();
        }

        #endregion

        #region Traits operation

        protected void RemoveTemporaryTraits()
        {
            character.RemoveTemporaryTraits();
        }

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

        public void TurnPreparationStop()
        {
            StopCoroutine(nameof(TurnPreparation));
        }

        virtual protected void StartOfTurn()
        {
            CombatManager.combatManager.PauseCombat();
            character.SendSignalData(new CombatSignalData(GameSignal.START_TURN, gameObject, CombatManager.combatManager.GetCharactersInCombat()));
            EvaluateTraits();
            DrawCard(1);
            turnTime = 0;
        }

        virtual public void EndTurn()
        {
            character.SendSignalData(new CombatSignalData(GameSignal.END_TURN, gameObject, CombatManager.combatManager.GetCharactersInCombat()));
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
    }
}