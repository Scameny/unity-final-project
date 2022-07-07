using Saving;
using Character.Stats;
using System.Collections.Generic;
using UnityEngine;
using Character.Classes;
using Items;
using CardSystem;
using Sirenix.OdinInspector;
using Abilities.Passive;

namespace Character.Character
{
    public class Hero : DefaultCharacter, ISaveable
    {
        [TabGroup("General")]
        [SerializeField] float exp;
        
        [TabGroup("Inventory")]
        [SerializeField] Inventory inventory = new Inventory();

        [TabGroup("Gear")]
        [HideLabel]
        [SerializeField] protected Gear gear;


        override protected void Start()
        {
            base.Start();
            OnGameStart();
        }

        private void OnGameStart()
        {
            AddAbilityCards(GetAllClassAbilitiesAvaliable());
            AddPassiveAbilities(GetClassPasiveAbilitiesAvaliable());
        }


        #region Abilities

        override public IEnumerable<Usable> GetUsableCards()
        {
            foreach (var item in base.GetUsableCards())
            {
                yield return item;
            }
            foreach (var item in gear.GetUsableCards())
            {
                yield return item;
            }
        }

        override public IEnumerable<Passive> GetCurrentPassiveAbilities()
        {
            foreach (var item in base.GetCurrentPassiveAbilities())
            {
                yield return item;
            }
            foreach (var item in gear.GetPasiveAbilities())
            {
                yield return item;
            }
        }

        private void AddPassiveAbilities(IEnumerable<Passive> passiveAbilities) 
        {
            foreach (var item in passiveAbilities)
            {
                permanentPassiveAbilities.Add(item);
            }
        }

        private void AddAbilityCards(IEnumerable<Usable> abilities)
        {
            foreach (var ability in abilities)
            {
                permanentCards.Add(ability);
            }
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

        #region Stats operations
        override public int GetStatistic(StatType type)
        {
            return base.GetStatistic(type) + gear.GetAdditiveModifier(type);
        }

        override public int GetSecondaryStatistic(DamageTypeStat type)
        {
            return base.GetSecondaryStatistic(type) + gear.GetAdditiveModifier(type);
        }

        #endregion

        #region Level operations
        public void AddExp(float expEarned)
        {
            exp += expEarned;
            if (level == ((PlayerClass)characterClass).GetMaxLevel())
                return;
            int expNeededToLvlUp = ((PlayerClass)characterClass).GetExpForNextLevel(level + 1);
            if (expNeededToLvlUp < exp)
            {
                exp -= expNeededToLvlUp;
            }
            LevelUp();
        }

        private void LevelUp()
        {
            level += 1;
            Debug.Log("Level up. Reached level " + level);
            foreach (var item in resources)
            {
                item.currentAmount = characterClass.GetMaxResourceAmount(level, item.resourceType);
            }
            AddAbilityCards(characterClass.GetAbilitiesOnLevel(level));
            AddPassiveAbilities(characterClass.GetPassiveAbilitiesOnLevel(level));
        }
        #endregion

        #region Items operations
        public void AddItems(List<Item> itemsEarned)
        {
            foreach (var item in itemsEarned)
            {
                try
                {
                    AddItem(item);
                } 
                catch (MaxItemQuantityException e)
                {
                    Debug.Log("Reached max quantity of item " + item.name);
                }
            }
        }


        public void AddItem(Item item, int i, int j)
        {
            inventory.AddItem(item, i, j);
        }

        public void AddItem(Item item)
        {
            inventory.AddItem(item);
        }

        /// <summary>
        /// Set and item in a slot and if there is already an item, is sent to the inventory
        /// </summary>
        /// <param name="slot"></param>
        /// <returns>True if the item has been equipped, false if not</returns>
        public bool SetItemBySlot(GearSlot slot, GearItem item)
        {
            if (gear.SetItemBySlot(slot, item))
            {
                if (item != null)
                    inventory.RemoveItem(item);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns all items in the inventory
        /// </summary>
        /// <returns></returns>
        public Item[,] GetAllStoredItems()
        {
            return inventory.items;
        }


        #endregion

        #region Saving system

        [System.Serializable]
        struct HeroData
        {
            public int level;
            public int currentHealth;
        }

        public void RestoreState(object state)
        {
            HeroData data = (HeroData)state;
            level = data.level;
        }

        public object CaptureState()
        {
            HeroData data = new HeroData();
            data.level = level;
            return data;
        }
        #endregion

        #region Debug
        [Button]
        public void LoadAbilities()
        {

            foreach (var item in GetAllClassAbilitiesAvaliable())
            {
                    permanentCards.Add(item);
            }
        }
        #endregion
    }
}

