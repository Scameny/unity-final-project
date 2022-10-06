using Character.Stats;
using Character.Character;
using CardSystem;
using GameManagement;
using UnityEngine;
using System.Collections;

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
            InitializeCardCotainers(GameObject.Find("Deck").GetComponent<Deck>(), GameObject.Find("Hand").GetComponentInChildren<Hand>(), GameObject.Find("Stack").GetComponentInChildren<CardSystem.Stack>());
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
        public override void CardUsed(Card card)
        {
            base.CardUsed(card);
        }

    }

}
