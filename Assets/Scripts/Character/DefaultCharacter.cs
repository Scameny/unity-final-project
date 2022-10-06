using System;
using Character.Classes;
using Character.Stats;
using Character.Buff;
using UnityEngine;
using Utils;
using CardSystem;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Abilities.Passive;
using Combat;
using System.Linq;
using GameManagement;
using Animations.Character;

namespace Character.Character 
{
    public class DefaultCharacter : SerializedMonoBehaviour, ICardGiver
    {
        [SerializeField] CharacterClass characterClass;

        Resource health;

        Resource armor;

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
        CharacterBuffs traits;
        PassiveManager passiveManager;

        private void Awake()
        {
            traits = GetComponent<CharacterBuffs>();
            passiveManager = new PassiveManager();
        }

        virtual protected void Start()
        {
            InitializeResources();
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
        public void RemoveCombatBuffs()
        {
            SendSignalData(traits.RemoveBuffs(gameObject, CombatManager.combatManager.GetCharactersInCombat()), true);
        }

        public void ReduceTurnInTemporaryBuffs()
        {
            SendSignalData(traits.ReduceTurnInTemporaryBuffs(gameObject, CombatManager.combatManager.GetCharactersInCombat()), true);
        }

        public List<SignalData> AddNewTrait(BaseBuff trait, bool sendUISignal = false)
        {
            List<SignalData> toRet = new List<SignalData>();
            switch (traits.NewBuff(trait))
            {
                case GameSignal.TRAIT_RENEWED:
                    toRet.Add(new TraitCombatSignalData(GameSignal.TRAIT_RENEWED, gameObject, CombatManager.combatManager.GetCharactersInCombat(), trait));
                    break;
                case GameSignal.TRAIT_MODIFIED:
                    toRet.Add(new TraitCombatSignalData(GameSignal.TRAIT_MODIFIED, gameObject, CombatManager.combatManager.GetCharactersInCombat(), trait));
                    toRet.AddRange(trait.GetSignalDatas(gameObject));
                    break;
                case GameSignal.NEW_TRAIT:
                    toRet.Add(new TraitCombatSignalData(GameSignal.NEW_TRAIT, gameObject, CombatManager.combatManager.GetCharactersInCombat(), trait));
                    toRet.AddRange(trait.GetSignalDatas(gameObject));
                    foreach (var item in trait.GetPasiveAbilities())
                    {
                        IDisposable disposable = passiveManager.Subscribe(item);
                        item.SetDisposable(disposable);
                    }
                    break;
                default:
                    break;
            }
            SendSignalData(toRet, sendUISignal);
            return toRet;
        }

        public List<SignalData> RemoveTrait(BaseBuff trait, bool sendUISignal = false)
        {
            List<SignalData> toRet = new List<SignalData>();
            if (traits.RemoveBuff(trait.GetName()))
            {
                toRet.Add(new TraitCombatSignalData(GameSignal.REMOVE_TRAIT, gameObject, CombatManager.combatManager.GetCharactersInCombat(), trait));
                toRet.AddRange(trait.GetSignalDatas(gameObject));
            }
            SendSignalData(toRet, sendUISignal);
            return toRet;
        }

        public List<SignalData> RemoveTrait(string buffName, bool sendUISignal = false)
        {
            List<SignalData> toRet = new List<SignalData>();
            try
            {
                BaseBuff buffToRemove = traits.GetBuff(buffName).buff;
                toRet.AddRange(RemoveTrait(buffToRemove, sendUISignal));
            }
            catch
            {
            }
            return toRet;
        }

        public BuffInfo GetBuff(string traitBuff)
        {
            return traits.GetBuff(traitBuff);
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

        public List<SignalData> UseResource(int resource, ResourceType resourceType, bool sendUISignal = false)
        {
            List<SignalData> toRet = new List<SignalData>();
            if (HaveEnoughResource(resource, resourceType))
            {
                SignalData resourceSignal = new CombatResourceSignalData(GameSignal.RESOURCE_MODIFY, gameObject, CombatManager.combatManager.GetCharactersInCombat(), resourceType, -resource, GetResourceByResourceType(resourceType).currentAmount);
                toRet.Add(resourceSignal);
                GetResourceByResourceType(resourceType).currentAmount -= resource;
                SendSignalData(toRet, sendUISignal);
            }
            else
            {
                throw new NotEnoughResourceException(resourceType);
            }
            return toRet;
        }

        void InitializeResources()
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
            List<SignalData> signals = new List<SignalData>();
            foreach (var item in characterClass.GetResourceTypes())
            {
                int maxResourceAmount = characterClass.GetMaxResourceAmount(level, item);
                if (characterClass.IsRechargeResource(item))
                    signals.AddRange(AddResource(item, maxResourceAmount));
                else
                    signals.AddRange(AddResource(item, maxResourceAmount, maxResourceAmount));
            }
            SendSignalData(signals, true);
            resources.Add(health);
            resources.Add(armor);
        }

        public bool HaveEnoughResource(int resourceAmount, ResourceType resourceType)
        {
            try
            {
                return GetResourceByResourceType(resourceType).currentAmount >= resourceAmount;
            } 
            catch (NotResourceTypeClassException)
            {
                return false;
            }
        }

        public List<SignalData> RemoveResource(ResourceType resourceType, bool sendUISignal = false)
        {
            List<SignalData> toRet = new List<SignalData>();
            foreach (var item in resources.ToList())
            {
                if (item.resourceType.Equals(resourceType))
                {
                    resources.Remove(item);
                    toRet.Add(new SignalData(GameSignal.REMOVE_RESOURCE));
                }
            }
            SendSignalData(toRet, sendUISignal);
            return toRet;
        }

        public bool HasResource(ResourceType resourceType)
        {
            foreach (var item in resources.ToList())
            {
                if (item.resourceType.Equals(resourceType))
                {
                    return true;
                }
            }
            return false;
        }

        public List<SignalData> AddResource(ResourceType resourceType, int maxAmount, int initialAmount = 0, bool sendUISignal = false)
        {
            List<SignalData> toRet = new List<SignalData>();
            foreach (var item in resources.ToList())
            {
                if (item.resourceType.Equals(resourceType))
                {
                    return toRet;
                }
            }
            Resource resource = new Resource();
            resource.resourceType = resourceType;
            resource.maxResource = maxAmount;
            resource.currentAmount = initialAmount;
            resources.Add(resource);
            toRet.Add(new SignalData(GameSignal.ADD_RESOURCE));
            SendSignalData(toRet, sendUISignal);
            return toRet;
        }


        public IEnumerable<ResourceType> GetResourceType()
        {
            return characterClass.GetResourceTypes();
        }

        public List<SignalData> GainResource(int amount, ResourceType resourceType, bool sendUISignal = false) 
        {
            List<SignalData> toRet = new List<SignalData>();
            Resource resource = GetResourceByResourceType(resourceType);
            resource.currentAmount += amount;
            resource.currentAmount = Mathf.Min(resource.currentAmount, resource.maxResource);
            if (resource.currentAmount < 0)
                resource.currentAmount = 0;
            SignalData resourceSignal = new CombatResourceSignalData(GameSignal.RESOURCE_MODIFY, gameObject, CombatManager.combatManager.GetCharactersInCombat(), resourceType, amount, resource.currentAmount);
            toRet.Add(resourceSignal);
            SendSignalData(toRet, sendUISignal);
            return toRet;
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

        public void ResetTemporaryResources()
        {
            List<SignalData> toRet = new List<SignalData>();
            foreach (var item in resources)
            {
                if (item.temporaryResource)
                {
                    toRet.Add(new ResourceSignalData(GameSignal.OUT_OF_COMBAT_CURRENT_RESOURCE_MODIFY, gameObject, item.resourceType, characterClass.GetMaxResourceAmount(level, item.resourceType), 0));
                    item.currentAmount = 0;
                }
            }
            SendSignalData(toRet, true);
        }

        #endregion

        #region health operations
        public bool IsDead()
        {
            return isDead;
        }

        public List<SignalData> TakeDamage(int damage, DamageType type, bool sendUISignal = false)
        {
            damage = ProcessDamageReceived(damage, type);
            List<SignalData> toRet = new List<SignalData>();
            if (damage > 0)
            {
                SignalData damageReceiveSignal = new DamageReceivedSignalData(GameSignal.DAMAGE_RECEIVED, gameObject, CombatManager.combatManager.GetCharactersInCombat(), damage);
                GetComponent<CharacterAnimation>().Hurt();
                toRet.Add(damageReceiveSignal);
                GameDebug.Instance.Log(Color.blue, gameObject.name + " taking " + damage + " damage");
                if (armor.currentAmount < damage)
                {
                    if (armor.currentAmount > 0)
                    {
                        SignalData armorSignal = new CombatResourceSignalData(GameSignal.RESOURCE_MODIFY, gameObject, CombatManager.combatManager.GetCharactersInCombat(), ResourceType.Armor, -armor.currentAmount, armor.currentAmount);
                        toRet.Add(armorSignal);
                        damage -= armor.currentAmount;
                        armor.currentAmount = 0;
                    }
                    SignalData healthSignal = new CombatResourceSignalData(GameSignal.RESOURCE_MODIFY, gameObject, CombatManager.combatManager.GetCharactersInCombat(), ResourceType.Health, -damage, health.currentAmount);
                    toRet.Add(healthSignal);
                    health.currentAmount -= damage;
                }
                else
                {
                    armor.currentAmount -= damage;
                    SignalData armorSignal = new CombatResourceSignalData(GameSignal.RESOURCE_MODIFY, gameObject, CombatManager.combatManager.GetCharactersInCombat(), ResourceType.Armor, -damage, armor.currentAmount);
                    toRet.Add(armorSignal);
                }
                SendSignalData(toRet, sendUISignal);
                if (health.currentAmount <= 0)
                {
                    Die();
                    SignalData dieSignal = new CombatSignalData(GameSignal.CHARACTER_DIE, gameObject, CombatManager.combatManager.GetCharactersInCombat());
                    toRet.Add(dieSignal);
                }
            }
            else
            {
                // Damage resisted signal
            }
            return toRet;
        }

        public List<SignalData> Heal(int healAmount, bool sendUISignal = false)
        {
            List<SignalData> toRet = new List<SignalData>();
            health.currentAmount += healAmount;
            if (health.currentAmount > health.maxResource)
                health.currentAmount = health.maxResource;
           
            SignalData healSignal = new CombatResourceSignalData(GameSignal.RESOURCE_MODIFY, gameObject, CombatManager.combatManager.GetCharactersInCombat(), ResourceType.Health, healAmount, health.currentAmount);
            toRet.Add(healSignal);
            SendSignalData(toRet, sendUISignal);
            GameDebug.Instance.Log(Color.green, gameObject.name + " was healed for " + healAmount + " points");
            return toRet;
        }

        private void Die()
        {
            if (isDead) return;
            isDead = true;
            GetComponent<CharacterAnimation>().Die();
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

        public void SendSignalData(SignalData data, bool sendUISignal = false)
        {
            passiveManager.SendData(data);
            if (sendUISignal)
                UI.UIManager.manager.SendData(data);
        }

        public void SendSignalData(List<SignalData> data, bool sendUISignal = false)
        {
            foreach (var item in data)
            {
                SendSignalData(item, sendUISignal);
            }
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

        #region setter y getters
        public List<Resource> GetResources()
        {
            return resources;
        }

        public CharacterClass GetClass()
        {
            return characterClass;
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
