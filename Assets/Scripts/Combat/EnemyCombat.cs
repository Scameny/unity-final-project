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
            character.currentHealth = character.GetStatistic(StatType.Health);
            character.maxHealth = character.currentHealth;
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
                TurnPreparationStop();
                Attack();
                prepared = false;
            }
            UpdateHealthBar();
        }

        private void Attack()
        {
            character.GetWeapon().attackDamage.ability.Use(gameObject, CombatManager.combatManager.charactersInCombat);
            EvaluateTraits();
        }
    }
}
