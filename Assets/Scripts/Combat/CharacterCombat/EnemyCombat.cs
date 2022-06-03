using UnityEngine;
using Character.Stats;
using Character.Character;

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
        void Update()
        {
            if (character.IsDead())
            {
                CombatManager.combatManager.EnemyDeath(this);
            }
            if (prepared)
            {
                prepared = false;
                TurnPreparationPause();
                DrawCard();
                UseCard();
                EvaluateTraits();
            }
            UpdateHealthBar();
        }

        #region Cards operations

        protected override void InitDeck()
        {
            CreateDeck();
            base.InitDeck();
        }

        private void CreateDeck()
        {
            foreach (var item in character.GetAllClassAbilitiesAvaliable())
            {
                for (int i = 0; i < item.quantity; i++)
                {
                    deck.CreateCard(gameObject, item.usable, false, cardPrefab);
                }
            }
            for (int i = 0; i < character.GetItemBySlot(Items.GearSlot.weapon).attackDamage.basicAttack.quantity; i++)
            {
                deck.CreateCard(gameObject, character.GetItemBySlot(Items.GearSlot.weapon).attackDamage.basicAttack.usable, false, cardPrefab);
            }
        }

        private void UseCard()
        {
            hand.GetNextCard().UseCard();
        }
        #endregion
    }
}
