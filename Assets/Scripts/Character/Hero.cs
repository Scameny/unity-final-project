using Saving;
using Character.Stats;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using Character.Classes;
using Items;
using CardSystem;
using Abilities.ability;

namespace Character.Character
{
    public class Hero : DefaultCharacter, ISaveable
    {
        [SerializeField] float exp;
        [SerializeField] Inventory inventory = new Inventory();
        [SerializeField] List<AbilityCard> extraAbiilities = new List<AbilityCard>();

        #region Stats operations
        override public float GetStatistic(StatType type)
        {
            return characterClass.GetStatistic(type, level) + gear.GetAdditiveModifier(type) + traits.GetAdditiveModifier(type);
        }

        override public float GetSecondaryStatistic(DamageTypeStat type)
        {
            return gear.GetAdditiveModifier(type) + traits.GetAdditiveModifier(type);
        }
        #endregion

        #region Level operations
        public void AddExp(float expEarned)
        {
            exp += expEarned;
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
            characterClass.GetAbilitiesOnLevel(level);
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
            GearItem itemToInv = GetItemBySlot(slot);
            if (gear.SetItemBySlot(slot, item))
            {
                inventory.RemoveItem(item, 1);
                if (itemToInv != null)
                    inventory.AddItem(itemToInv);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns all items in the inventory
        /// </summary>
        /// <returns></returns>
        public List<Item> GetAllStoredItems()
        {
            return inventory.gearItems;
        }


        #endregion

        #region Saving system

        [System.Serializable]
        struct HeroData
        {
            public string heroClass;
            public int level;
            public float currentHealth;
        }

        public void RestoreState(object state)
        {
            HeroData data = (HeroData)state;
            characterClass = Resources.Load<PlayerClass>(data.heroClass);
            level = data.level;
            currentHealth = data.currentHealth;
        }

        public object CaptureState()
        {
            HeroData data = new HeroData();
            data.heroClass = characterClass.name;
            data.level = level;
            data.currentHealth = currentHealth;
            return data;
        }
        #endregion
    }
}

