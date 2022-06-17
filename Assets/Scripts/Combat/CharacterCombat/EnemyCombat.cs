using UnityEngine;
using Character.Stats;
using Character.Character;
using CardSystem;

namespace Combat
{
    public class EnemyCombat : TurnCombat
    {
        HeroCombat target;
        // Data of the enemy class , level, etc..


        private void Awake()
        {
            character = GetComponent<Npc>();
        }

        private void Start()
        {
            target = GameObject.FindGameObjectWithTag("Player").GetComponent<HeroCombat>();
            turnSpeed = character.GetStatistic(StatType.Agility);
        }

        // Update is called once per frame
        override protected void Update()
        {
            if (character.IsDead())
            {
                CombatManager.combatManager.EnemyDeath(this);
            }
            if (prepared)
            {
                prepared = false;
                TurnPreparationPause();
                DrawCard(1);
                UseCard();
            }
            base.Update();
        }

        #region Cards operations

        protected override void InitDeck()
        {
            base.InitDeck();
        }

        private void UseCard()
        {
            try
            {
                Card card = hand.RemoveNextCard();
                card.UseCard();
            } catch (EmptyCardContainerException e) 
            {
                TurnPreparationResume();
            }
        }
        #endregion
    }
}
