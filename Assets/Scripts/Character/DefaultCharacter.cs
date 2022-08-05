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
using Combat;
using System.Linq;
using GameManagement;

namespace Character.Character 
{
    public class DefaultCharacter : SerializedMonoBehaviour, ICardGiver
    {
        [SerializeField] protected CharacterClass characterClass;

        protected Resource health;

        protected Resource armor;

        [TabGroup("General")]
        [SerializeField] protected List<Resource> resources = new List<Resource>();


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
        PassiveManager passiveManager;

        private void Awake()
        {
            traits = GetComponent<Traits>();
        }

        virtual protected void Start()
        {
            passiveManager = new PassiveManager();
            InitializeResources();
        }

        private void Update()
        {
            foreach (var item in resources)
            {
                if (!item.temporaryResource)
                    item.maxResource = characterClass.GetMaxResourceAmount(level, item.resourceType);
            }
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

        public void RemovePermanentCard(Usable usable)
        {
            permanentCards.Remove(usable);
        }

        public IEnumerable<Usable> GetPermanentCards()
        {
            foreach (var item in permanentCards)
            {
                yield return item;
            }
        }

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
        public void RemoveTemporaryTraits()
        {
            traits.RemoveTemporaryTraits();
        }

        public void ReduceTurnInTemporaryTraits()
        {
            traits.ReduceTurnInTemporaryTraits();
        }

        public void AddNewTrait(BaseTrait trait)
        {
            if (traits.NewTrait(trait))
            {
                foreach (var item in trait.GetPasiveAbilities())
                {
                    IDisposable disposable = passiveManager.Subscribe(item);
                    item.SetDisposable(disposable);
                }
            }
        }

        #endregion

        #region Stats operations
        public int GetMaxCardsInHand()
        {
            return characterClass.GetMaxCardsHand(level);
        }


        public virtual int GetStatistic(StatType type)
        {
            return characterClass.GetStatistic(type, level) + traits.GetAdditiveModifier(type);
        }

        public virtual int GetSecondaryStatistic(DamageTypeStat type)
        {
            return traits.GetAdditiveModifier(type);
        }

        protected int GetOffensiveStatViaDamageType(DamageType damageType)
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

        protected int GetDefensiveSecondaryStatViaDamageType(DamageType damageType)
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

        protected int ProcessDamageReceived(int damage, DamageType damageType)
        {
            return Mathf.Max(0, damage - GetDefensiveSecondaryStatViaDamageType(damageType));
        }

        public int ProcessDamageDone(int damage, DamageType damageType)
        {
            return damage + GetOffensiveStatViaDamageType(damageType);
        }

        public void UseResource(int resource, ResourceType resourceType)
        {
            if (HaveEnoughResource(resource, resourceType))
            {
                SendSignalData(new ResourceSignalData(GameSignal.RESOURCE_MODIFY, gameObject, CombatManager.combatManager.GetCharactersInCombat(), resourceType, -resource, GetResourceByResourceType(resourceType).currentAmount));
                GetResourceByResourceType(resourceType).currentAmount -= resource;
            }
            else
            {
                throw new NotEnoughResourceException(resourceType);
            }
        }

        protected void InitializeResources()
        {
            health = new Resource();
            health.resourceType = ResourceType.Health;
            health.maxResource = GetStatistic(StatType.Health);
            health.currentAmount = GetStatistic(StatType.Health);
            armor = new Resource();
            armor.resourceType = ResourceType.Armor;
            // Use constants
            armor.maxResource = 999;
            armor.currentAmount = 0;
            armor.temporaryResource = true;
            foreach (var item in characterClass.GetResourceTypes())
            {
                Resource resource = new Resource();
                resource.resourceType = item;
                resource.maxResource = characterClass.GetMaxResourceAmount(level, item);
                resource.currentAmount = resource.maxResource;
                resources.Add(resource);
            }
            resources.Add(health);
            resources.Add(armor);
        }

        public bool HaveEnoughResource(int resourceAmount, ResourceType resourceType)
        {
            return GetResourceByResourceType(resourceType).currentAmount >= resourceAmount;
        }


        public IEnumerable<ResourceType> GetResourceType()
        {
            return characterClass.GetResourceTypes();
        }

        public void GainResource(int amount, ResourceType resourceType) 
        {
            Resource resource = GetResourceByResourceType(resourceType);
            SendSignalData(new ResourceSignalData(GameSignal.RESOURCE_MODIFY, gameObject, CombatManager.combatManager.GetCharactersInCombat(), resourceType, amount, resource.currentAmount));
            resource.currentAmount += amount;
            resource.currentAmount = Mathf.Min(resource.currentAmount, resource.maxResource);
        }


        protected Resource GetResourceByResourceType(ResourceType resourceType)
        {
            if (resources.Any(r => r.resourceType.Equals(resourceType)))
            {
                return resources.Find(r => r.resourceType.Equals(resourceType));
            }
            else
            {
                throw new NotResourceTypeClassException();
            }
        }

        public int GetCurrentResource(ResourceType resourceType) 
        {
            return GetResourceByResourceType(resourceType).currentAmount;   
        }

        public int GetMaxValueOfResource(ResourceType resourceType)
        {
            return GetResourceByResourceType(resourceType).maxResource;
        }

        #endregion

        #region health operations
        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(int damage, DamageType type)
        {
            damage = ProcessDamageReceived(damage, type);
            if (damage > 0)
            {
                SendSignalData(new DamageReceivedSignalData(GameSignal.DAMAGE_RECEIVED, gameObject, CombatManager.combatManager.GetCharactersInCombat(), damage));
                GameDebug.Instance.Log(Color.blue, gameObject.name + " taking " + damage + " damage");
                if (armor.currentAmount < damage)
                {
                    if (armor.currentAmount > 0)
                    {
                        SendSignalData(new ResourceSignalData(GameSignal.RESOURCE_MODIFY, gameObject, CombatManager.combatManager.GetCharactersInCombat(), ResourceType.Armor, -armor.currentAmount, armor.currentAmount));
                        damage -= armor.currentAmount;
                        armor.currentAmount = 0;
                    }
                    SendSignalData(new ResourceSignalData(GameSignal.RESOURCE_MODIFY, gameObject, CombatManager.combatManager.GetCharactersInCombat(), ResourceType.Health, -damage, health.currentAmount));
                    health.currentAmount -= damage;
                }
                else
                {
                    armor.currentAmount -= damage;
                    SendSignalData(new ResourceSignalData(GameSignal.RESOURCE_MODIFY, gameObject, CombatManager.combatManager.GetCharactersInCombat(), ResourceType.Armor, -damage, armor.currentAmount));
                }
                GetComponent<Animator>().Play("Hurt");
                if (health.currentAmount <= 0)
                    Die();
            }
        }

        public void Heal(int healAmount)
        {
            SendSignalData(new ResourceSignalData(GameSignal.RESOURCE_MODIFY, gameObject, CombatManager.combatManager.GetCharactersInCombat(), ResourceType.Health, healAmount, health.currentAmount));
            health.currentAmount += healAmount;
            GameDebug.Instance.Log(Color.green, gameObject.name + " was healed for " + healAmount + " points");
            if (health.currentAmount > health.maxResource)
                health.currentAmount = health.maxResource;
        }

        private void Die()
        {
            if (isDead) return;
            isDead = true;
            GetComponent<Animator>().Play("Die");
            GameDebug.Instance.Log(Color.red, gameObject.name + " dies");
            // Animation
        }

        public int GetCurrentHealth()
        {
            return health.currentAmount;
        }

        public int GetMaxHealth()
        {
            return health.maxResource;
        }
        #endregion

        #region Signal operations

        public void SendSignalData(SignalData data)
        {
            passiveManager.SendData(data);
            UI.UIManager.manager.SendData(data);
        }

        public void DisposePassiveAbilities()
        {
            passiveManager.Unsubscribe();
        }

        public void ActivePassiveAbilities()
        {
            foreach (var item in GetCurrentPassiveAbilities())
            {
                IDisposable disposable = passiveManager.Subscribe(item);
                item.SetDisposable(disposable);
            }
        }

        #endregion

    }

    public class Resource
    {
        [LabelText("Resource type")]
        public ResourceType resourceType;
        [LabelText("Current resource")]
        public int currentAmount;
        [LabelText("Max resource")]
        public int maxResource;
        public bool temporaryResource;
    }

}
