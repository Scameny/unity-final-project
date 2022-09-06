using UnityEngine;
using Character.Stats;
using System.Collections;
using Character.Character;
using CardSystem;
using GameManagement;

namespace Combat 
{
    public class HeroCombat : TurnCombat
    {
        private void Awake()
        {
            character = GetComponent<Hero>();
        }
        
        private void Start()
        {
            turnSpeed = character.GetStatistic(StatType.Agility);
        }

        public override void StartCombat()
        {
            base.StartCombat();
        }

        public override void EndCombat()
        {
            character.SendSignalData(new CombatSignalData(GameSignal.END_COMBAT, gameObject, CombatManager.combatManager.GetCharactersInCombat()), true);
            base.EndCombat();
        }

        override protected void Update()
        {
            if (character.IsDead())
            {
                CombatManager.combatManager.HeroDeath();
            }
            base.Update();
        }

        public override void TurnPreparationStart()
        {
            base.TurnPreparationStart();
            character.SendSignalData(new CombatSignalData(GameSignal.TURN_PREPARATION_START, gameObject, CombatManager.combatManager.GetCharactersInCombat()), true);
        }


        public override void EndTurn()
        {
            base.EndTurn();
        }

        public override void CardUsed(Card card)
        {
            base.CardUsed(card);
        }
    }

}
