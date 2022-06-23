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

        override protected void Update()
        {
            if (character.IsDead())
            {
                CombatManager.combatManager.HeroDeath();
            }
            base.Update();
        }

        public void StartCoroutine(Action<IEnumerator> coroutine)
        {
            StartCoroutine(coroutine);
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
            base.EndTurn();
            UIManager.manager.CombatUIInteractable(false);
            CombatManager.combatManager.ResumeCombat();
        }

        protected override void StartOfTurn()
        {
            base.StartOfTurn();
            UIManager.manager.CombatUIInteractable(true);
            CombatManager.combatManager.PauseCombat();
        }

    }

}
