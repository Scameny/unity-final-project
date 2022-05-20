using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using Character.Character;
using Character.Abilities;

namespace Combat
{
    /// <summary>
    /// Class that should inherit every script that manage combat.
    /// When enabled, it starts the turn based system.
    /// </summary>
    public class TurnCombat : MonoBehaviour
    {
        public Slider healthBar;
        public GameObject selector;
        public GameObject traitRow;
        [HideInInspector]
        public DefaultCharacter character = null;
        protected bool prepared = false;
        protected float turnSpeed;
        protected float turnTime;
        protected bool stopTurn;

        private void OnEnable()
        {
            TurnPreparationStart();
            EnableHealthBar(true);
        }

        private void OnDisable()
        {
            EnableHealthBar(false);
        }

        public void TakeDamage(float damage, DamageType type)
        {
            character.TakeDamage(damage, type);
        }

        protected void EvaluateTraits()
        {
            character.traits.ReduceTurnInTemporaryTraits();
        }

        #region Turn management
        public void TurnPreparationStart()
        {
            turnTime = 0.0f;
            stopTurn = false;
            StartCoroutine(nameof(TurnPreparation));
        }

        public virtual void TurnPreparationResume()
        {
            Debug.Log(gameObject.name + ": turn resume");
            stopTurn = false;
        }

        public void TurnPreparationStop()
        {
            Debug.Log(gameObject.name + ": turn pause");
            stopTurn = true;
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
                    else
                    {
                        prepared = true;
                        stopTurn = true;
                        turnTime = 0;
                    }
                }
                yield return new WaitForSeconds(0.05f);                
            }
            
        }
        #endregion


        #region UI Management
        protected void EnableHealthBar(bool enable)
        {
            healthBar.gameObject.SetActive(true);
        }


        protected void UpdateHealthBar()
        {
            healthBar.maxValue = character.maxHealth;
            healthBar.value = character.currentHealth;
        }

        #endregion

    }

}