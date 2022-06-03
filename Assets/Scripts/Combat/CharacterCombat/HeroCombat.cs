using System;
using UnityEngine;
using Character.Stats;
using System.Collections;
using UI;
using Character.Character;
using Utils;
using CardSystem;
using NaughtyAttributes;

namespace Combat 
{
    public class HeroCombat : TurnCombat
    {
        [HideInInspector]
        public int targetIndex;


        private void Awake()
        {
            character = GetComponent<Hero>();
        }
        
        private void Start()
        {
            targetIndex = 0;
            turnSpeed = character.GetStatistic(StatType.Agility);
        }
        public void StartCoroutine(Action<IEnumerator> coroutine)
        {
            StartCoroutine(coroutine);
        }

        private void Update()
        {
            if (character.IsDead())
            {
                CombatManager.combatManager.HeroDeath();
            }
            else if (prepared)
            {
                prepared = false;
                DrawCard();
                UIManager.manager.ActivateCombatUI(true);
                CombatManager.combatManager.PauseCombat();
            }
            UpdateHealthBar();
        }

        #region Card operations
        override protected void InitDeck()
        {
            deck.AddPermanentDeckToCurrentDeck();
            foreach (var item in ((Hero)character).GetUsableCards())
            {
                for (int i = 0; i < item.quantity; i++)
                {
                    deck.CreateCard(gameObject, item.usable, true, false, cardPrefab);
                }
            }
        }
        #endregion

        override public void TurnPreparationResume()
        {
            stopTurn = false;
            CombatManager.combatManager.ResumeCombat();
        }

        #region Debug
        [Button]
        public void LoadAbilities()
        {

            foreach (var item in character.GetAllClassAbilitiesAvaliable())
            {
                for (int i = 0; i < item.quantity; i++)
                {
                    deck.CreateCard(gameObject, item.usable, false, false,cardPrefab);
                }
            }
        }
        #endregion

    }

}
