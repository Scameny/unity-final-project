using System;
using Character.Classes;
using Character.Stats;
using Character.Abilities;
using Character.Trait;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using Items;

namespace Character.Character 
{
    public class DefaultCharacter : MonoBehaviour
    {
        public CharacterClass characterClass;
        public List<AbilityUsable> abilitiesAvaliable = new List<AbilityUsable>();
        public Gear gear;
        public Traits traits;
        public int level;

        [Header("heal")]
        public float currentHealth;
        public float maxHealth;
        bool isDead;

        private void Awake()
        {
            traits = GetComponent<Traits>();
        }

        public virtual float GetStatistic(StatType type)
        {
            throw new NotImplementedException();
        }

        public virtual float GetSecondaryStatistic(DamageTypeStat type)
        {
            throw new NotImplementedException();
        }

        public GearItem GetWeapon()
        {
            if (gear.weapon != null)
            {
                return gear.weapon.item;
            }
            else
            {
                throw new MissingRequiredParameterException(gear.weapon.ToString(), this.ToString());
            }
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
        #endregion

    }

}
