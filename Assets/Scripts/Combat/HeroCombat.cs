using System;
using UnityEngine;
using Character.Stats;
using System.Collections;
using UI;
using Character.Character;
using Character.Abilities;
using Items;
using Utils;

namespace Combat 
{
    public class HeroCombat : TurnCombat
    {
        [HideInInspector]
        public int targetIndex;
        bool waitingForAction = false;

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
            else if (prepared && !waitingForAction)
            {
                UIManager.manager.ActivateCombatMenu();
                waitingForAction = true;
                CombatManager.combatManager.PauseCombat();
            }
            UpdateHealthBar();
            //if (stopTurn)
                //EvaluateTraits(Time.deltaTime);
        }

        public void Attack()
        {
            if (prepared)
            {
                character.GetWeapon().attackDamage.ability.Use(gameObject, CombatManager.combatManager.charactersInCombat);
                EvaluateTraits();
                prepared = false;
                waitingForAction = false;
            }
            else
            {
                Debug.LogError("Character not ready to perfom an action");
            }
        }
       

        public void UseAbility(AbilityUsable ability)
        {
            if (prepared)
            {
                ability.UseAbility(gameObject, CombatManager.combatManager.charactersInCombat);
                EvaluateTraits();
                prepared = false;
                waitingForAction = false;
            }
            else
            {
                GameDebug.Instance.LogError("Character not ready to perfom an action");
            }
        }

        public void UseItem(UsableItem item)
        {
            if (prepared)
            {
                ((Hero)character).inventory.UseItem(item, gameObject, CombatManager.combatManager.charactersInCombat);
                EvaluateTraits();
                prepared = false;
                waitingForAction = false;
            }
            else
            {
                GameDebug.Instance.LogError("Character not ready to perfom an action");

            }
        }

        override public void TurnPreparationResume()
        {
            stopTurn = false;
            CombatManager.combatManager.ResumeCombat();
        }

    }

}
