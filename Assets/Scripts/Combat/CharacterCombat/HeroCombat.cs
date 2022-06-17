using System;
using UnityEngine;
using Character.Stats;
using System.Collections;
using UI;
using Character.Character;

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

        override protected void Update()
        {
            if (character.IsDead())
            {
                CombatManager.combatManager.HeroDeath();
            }
            else if (prepared)
            {
                prepared = false;
                DrawCard(1);
                UIManager.manager.ActivateCombatUI(true);
                CombatManager.combatManager.PauseCombat();
            }
            base.Update();
        }

        public void EndTurn()
        {
            TurnPreparationResume();
        }

        #region Card operations
        override protected void InitDeck()
        {
            base.InitDeck();
        }
        #endregion

        override public void TurnPreparationResume()
        {
            stopTurn = false;
            CombatManager.combatManager.ResumeCombat();
        }

    }

}
