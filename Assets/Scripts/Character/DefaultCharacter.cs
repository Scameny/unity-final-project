using System;
using Character.Classes;
using Character.Stats;
using Character.Trait;
using UnityEngine;
using Utils;
using Items;
using CardSystem;
using System.Collections.Generic;
using Abilities.ability;

namespace Character.Character 
{
    public class DefaultCharacter : MonoBehaviour
    {
        public CharacterClass characterClass;
        
        [Header("Modifiers")]
        [SerializeField] protected Gear gear;
        [SerializeField] protected Traits traits;


        [Header("heal")]
        [SerializeField] protected float currentHealth;
        [SerializeField] protected float maxHealth;


        [Header("Level")]
        [SerializeField] protected int level;

        bool isDead;

        private void Awake()
        {
            traits = GetComponent<Traits>();
        }

        #region Abilities operations
        public List<AbilityCard> GetAllClassAbilitiesAvaliable()
        {
            return characterClass.GetAllAbilitesAvaliable(level);
        }
        #endregion


        #region Gear operations
        /// <summary>
        /// Return item in the specified slot
        /// </summary>
        /// <param name="slot"></param>
        /// <returns></returns>
        public GearItem GetItemBySlot(GearSlot slot)
        {
            return gear.GetItemBySlot(slot);
        }
        #endregion

        #region Traits operations
        public void ReduceTurnInTemporaryTraits()
        {
            traits.ReduceTurnInTemporaryTraits();
        }

        public void AddNewTrait(BaseTrait trait)
        {
            traits.NewTrait(trait);
        }

        #endregion

        #region Stats operations
        public int GetMaxCardsInHand()
        {
            return characterClass.GetMaxCardsHand(level);
        }


        public virtual float GetStatistic(StatType type)
        {
            throw new NotImplementedException();
        }

        public virtual float GetSecondaryStatistic(DamageTypeStat type)
        {
            throw new NotImplementedException();
        }

        protected float GetOffensiveStatViaDamageType(DamageType damageType)
        {
            switch (damageType)
            {
                case DamageType.Physic:
                    return GetSecondaryStatistic(DamageTypeStat.PhysicDamage);
                case DamageType.Fire:
                    return GetSecondaryStatistic(DamageTypeStat.FireDamage);
                case DamageType.Ice:
                    return GetSecondaryStatistic(DamageTypeStat.IceDamage);
                case DamageType.Electic:
                    return GetSecondaryStatistic(DamageTypeStat.ElecticDamage);
                case DamageType.Arcane:
                    return GetSecondaryStatistic(DamageTypeStat.ArcaneDamage);
                case DamageType.Nature:
                    return GetSecondaryStatistic(DamageTypeStat.NatureDamage);
                default:
                    break;
            }
            throw new KeyValueMissingException(damageType.ToString(), this.name);
        }

        protected float GetDefensiveSecondaryStatViaDamageType(DamageType damageType)
        {
            switch (damageType)
            {
                case DamageType.Physic:
                    return GetSecondaryStatistic(DamageTypeStat.PhysicResistance);
                case DamageType.Fire:
                    return GetSecondaryStatistic(DamageTypeStat.FireResistance);
                case DamageType.Ice:
                    return GetSecondaryStatistic(DamageTypeStat.IceResistance);
                case DamageType.Electic:
                    return GetSecondaryStatistic(DamageTypeStat.ElecticResistance);
                case DamageType.Arcane:
                    return GetSecondaryStatistic(DamageTypeStat.ArcaneResistance);
                case DamageType.Nature:
                    return GetSecondaryStatistic(DamageTypeStat.NatureResistance);
                default:
                    break;
            }
            throw new KeyValueMissingException(damageType.ToString(), this.name);
        }

        protected float ProcessDamageReceived(float damage, DamageType damageType)
        {
            return Mathf.Max(0, damage - GetDefensiveSecondaryStatViaDamageType(damageType));
        }

        public float ProcessDamageDone(float damage, DamageType damageType)
        {
            return damage + GetOffensiveStatViaDamageType(damageType);
        }
        #endregion

        #region health operations
        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(float damage, DamageType type)
        {
            damage = Mathf.Ceil(ProcessDamageReceived(damage, type));
            currentHealth -= damage;
            GameDebug.Instance.Log(Color.blue, gameObject.name + " taking " + damage + " damage");
            if (currentHealth <= 0)
                Die();
        }

        public void Heal(float healAmount)
        {
            currentHealth += healAmount;
            GameDebug.Instance.Log(Color.green, gameObject.name + " was healed for " + healAmount + " points");
            if (currentHealth > maxHealth)
                currentHealth = maxHealth;
        }

        private void Die()
        {
            if (isDead) return;
            isDead = true;
            GameDebug.Instance.Log(Color.red, gameObject.name + " dies");
            // Animation
        }

        public float GetCurrentHealth()
        {
            return currentHealth;
        }

        public float GetMaxHealth()
        {
            return maxHealth;
        }
        #endregion

    }

}
