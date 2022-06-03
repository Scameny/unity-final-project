using Abilities.BasicAttack;
using CardSystem;
using Character.Stats;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    [System.Serializable]
    public class Gear
    {

        [SerializeField] GearItemSlots head = new GearItemSlots(GearPiece.Helm);
        [SerializeField] GearItemSlots chest = new GearItemSlots(GearPiece.Chest);
        [SerializeField] GearItemSlots legs = new GearItemSlots(GearPiece.Legs);
        [SerializeField] GearItemSlots weapon = new GearItemSlots(GearPiece.Weapon);
        [SerializeField] GearItemSlots gloves = new GearItemSlots(GearPiece.Gloves);
        [SerializeField] GearItemSlots firstRing = new GearItemSlots(GearPiece.Ring);
        [SerializeField] GearItemSlots secondRing=  new GearItemSlots(GearPiece.Ring);

        #region Stats operations
        /// <summary>
        /// Return the value given by gear of the specific stat
        /// </summary>
        public float GetAdditiveModifier(StatType stat)
        {
            GearItemSlots[] items = new GearItemSlots[7] { head, chest, legs, weapon, gloves, firstRing, secondRing };
            float value = 0;
            foreach (var slot in items)
            {
                if (slot.GetItem() != null) { 
                    foreach (var statValue in slot.GetItem().GetAdditiveModifier(stat))
                    {
                         value += statValue;
                    }
                }
            }
            return value;
        }

        /// <summary>
        /// Return the value given by gear of the specific stat
        /// </summary
        public float GetAdditiveModifier(DamageTypeStat stat)
        {
            GearItemSlots[] items = new GearItemSlots[7] { head, chest, legs, weapon, gloves, firstRing, secondRing };
            float value = 0;
            foreach (var slot in items)
            {
                if (slot.GetItem() != null)
                {
                    foreach (var statValue in slot.GetItem().GetAdditiveModifier(stat))
                    {
                        value += statValue;
                    }
                }
            }
            return value;
        }

        #endregion

        #region Items operatiosn
        /// <summary>
        /// Return the item equiped by slot
        /// </summary>
        public GearItem GetItemBySlot(GearSlot slot)
        {
            switch (slot)
            {
                case GearSlot.helm:
                    return head.GetItem();
                case GearSlot.gloves:
                    return gloves.GetItem();
                case GearSlot.chest:
                    return chest.GetItem();
                case GearSlot.legs:
                    return legs.GetItem();
                case GearSlot.firstRing:
                    return firstRing.GetItem();
                case GearSlot.secondRing:
                    return secondRing.GetItem();
                case GearSlot.weapon:
                    return weapon.GetItem();
            }
            throw new KeyValueMissingException(slot.ToString(), GetType().Name);
        }

        /// <summary>
        /// Set the item in the specified slot.
        /// </summary>
        /// <param name="slot"></param>
        /// <param name="item"></param>
        /// <returns>Return true when the item is for the specified slot</returns>
        public bool SetItemBySlot(GearSlot slot, GearItem item)
        {
            switch (slot)
            {
                case GearSlot.helm:
                    return head.SetItem(item);
                case GearSlot.gloves:
                    return gloves.SetItem(item);
                case GearSlot.chest:
                    return chest.SetItem(item);
                case GearSlot.legs:
                    return legs.SetItem(item);
                case GearSlot.firstRing:
                    return firstRing.SetItem(item);
                case GearSlot.secondRing:
                    return secondRing.SetItem(item);
                case GearSlot.weapon:
                    return weapon.SetItem(item);
            }
            throw new KeyValueMissingException(slot.ToString(), GetType().Name);
        }
        #endregion

        #region Abilities operations
        
        public List<UsableCard> GetAbilitiesGivenByGear()
        {
            GearItemSlots[] items = new GearItemSlots[7] { head, chest, legs, weapon, gloves, firstRing, secondRing };
            List<UsableCard> cards = new List<UsableCard>();
            foreach (var item in items)
            {
                foreach (var usableCard in item.GetItem().GetUsableCards())
                {
                    cards.Add(usableCard);
                }
            }
            return cards;
        }
        #endregion
    }

    [System.Serializable]
    public class GearItemSlots
    {
        [SerializeField] GearItem item = null;
        [HideInInspector]
        public GearPiece slotType { get; private set; }

        public GearItemSlots(GearPiece slotType)
        {
            this.slotType = slotType;
        }

        public bool SetItem(GearItem item)
        {
            if (item != null)
            {
                if (item.slot != slotType)
                    return false;
                this.item = item;
            }
            else
            {
                this.item = null;
            }
            return true;
        }

        public GearItem GetItem()
        {
            return item;
        }

        
    }

    public enum GearSlot
    {
        helm, gloves, chest, legs, firstRing, secondRing, weapon
    }
}

