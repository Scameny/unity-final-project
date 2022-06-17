using System;
using Character.Classes;
using Character.Stats;
using Character.Trait;
using UnityEngine;
using Utils;
using CardSystem;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Abilities.Passive;

namespace Character.Character 
{
    public class DefaultCharacter : SerializedMonoBehaviour, ICardGiver
    {
        [SerializeField] protected CharacterClass characterClass;

        [Header("Heal")]
        [TabGroup("General")]
        [LabelText("Current health")]
        [SerializeField] protected float currentHealth;

        [TabGroup("General")]
        [LabelText("Max health")]
        [SerializeField] protected float maxHealth;

        [Header("@GetResourceType().ToString()")]
        [TabGroup("General")]
        [LabelText("Current resource")]
        [SerializeField] protected float currentResource;

        [TabGroup("General")]
        [LabelText("Max resource")]
        [SerializeField] protected float maxResource;


        [Header("Level")]
        [TabGroup("General")]
        [SerializeField] protected int level;


        [Header("Cards")]
        [TabGroup("Cards")]
        [SerializeField] protected List<Usable> permanentCards = new List<Usable>();

        [TabGroup("Passive abilities")]
        [SerializeField] protected List<Passive> permanentPassiveAbilities = new List<Passive>();

        bool isDead;
        protected Traits traits;

        private void Awake()
        {
            traits = GetComponent<Traits>();
        }

        #region Abilities operations

        public IEnumerable<Usable> GetAllClassAbilitiesAvaliable()
        {
            return characterClass.GetAllAbilitesAvaliable(level);
        }

        protected IEnumerable<Passive> GetClassPasiveAbilitiesAvaliable()
        {
            return characterClass.GetAllPassiveAbilitiesAvaliable(level);
        }

        virtual public IEnumerable<Passive> GetCurrentPassiveAbilities()
        {
            foreach (var item in permanentPassiveAbilities)
            {
                yield return item;
            }
            foreach (var item in traits.GetPasiveAbilities())
            {
                yield return item;
            }
        }

        #endregion

        #region Cards operations
        virtual public IEnumerable<Usable> GetUsableCards()
        {
            foreach (var card in traits.GetUsableCards())
            {
                yield return card;
            }
            foreach (var card in permanentCards)
            {
                yield return card;
            }
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
            return characterClass.GetStatistic(type, level) + traits.GetAdditiveModifier(type);
        }

        public virtual float GetSecondaryStatistic(DamageTypeStat type)
        {
            return traits.GetAdditiveModifier(type);
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

        public void UseResource(int resource, ResourceType resourceType)
        {
            currentResource -= resource;    
        }

        public bool HaveEnoughResource(int resourceAmount, ResourceType resourceType)
        {
            Debug.Log("Comprobando que tienes recursos coste:" + resourceAmount + " Mana actual : " + currentResource);
            return resourceAmount < currentResource;
        }

        public ResourceType GetResourceType()
        {
            return characterClass.GetResourceType();
        }

        public void GainResource(int amount) 
        {
            currentResource += amount;
            currentResource = Mathf.Min(currentResource, maxResource);
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
