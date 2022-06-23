using Abilities.Passive;
using CardSystem;
using Character.Stats;
using Character.Trait;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    [System.Serializable]
    [InlineProperty]
    public class Gear: ICardGiver, IPassiveProvider
    {
        [HorizontalGroup("Main")]

        [ValidateInput("ValidateEquipment")]
        [LabelWidth(100)]
        [SerializeField] GearItemSlots head = new GearItemSlots(GearPiece.Helm);
        [ValidateInput("ValidateEquipment")]
        [LabelWidth(100)]
        [SerializeField] GearItemSlots chest = new GearItemSlots(GearPiece.Chest);
        [ValidateInput("ValidateEquipment")]
        [LabelWidth(100)]
        [SerializeField] GearItemSlots legs = new GearItemSlots(GearPiece.Legs);
        [ValidateInput("ValidateEquipment")]
        [LabelWidth(100)]
        [SerializeField] GearItemSlots weapon = new GearItemSlots(GearPiece.Weapon);
        [ValidateInput("ValidateEquipment")]
        [LabelWidth(100)]
        [SerializeField] GearItemSlots gloves = new GearItemSlots(GearPiece.Gloves);
        [ValidateInput("ValidateEquipment")]
        [LabelWidth(100)]
        [SerializeField] GearItemSlots firstRing = new GearItemSlots(GearPiece.Ring);
        [ValidateInput("ValidateEquipment")]
        [LabelWidth(100)]
        [SerializeField] GearItemSlots secondRing=  new GearItemSlots(GearPiece.Ring);

        #region Stats operations
        /// <summary>
        /// Return the value given by gear of the specific stat
        /// </summary>
        public int GetAdditiveModifier(StatType stat)
        {
            GearItemSlots[] items = new GearItemSlots[7] { head, chest, legs, weapon, gloves, firstRing, secondRing };
            int value = 0;
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
        public int GetAdditiveModifier(DamageTypeStat stat)
        {
            GearItemSlots[] items = new GearItemSlots[7] { head, chest, legs, weapon, gloves, firstRing, secondRing };
            int value = 0;
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

        public IEnumerable<Usable> GetUsableCards()
        {
            GearItemSlots[] items = new GearItemSlots[7] { head, chest, legs, weapon, gloves, firstRing, secondRing };
            foreach (var item in items)
            {
                if (item.GetItem() != null)
                {
                    foreach (var usableCard in item.GetItem().GetUsableCards())
                    {
                        yield return usableCard;
                    }
                }
            }
        }

        public IEnumerable<Passive> GetPasiveAbilities()
        {
            GearItemSlots[] items = new GearItemSlots[7] { head, chest, legs, weapon, gloves, firstRing, secondRing };
            foreach (var item in items)
            {
                if (item.GetItem() != null)
                {
                    foreach (var passive in item.GetItem().GetPasiveAbilities())
                    {
                        yield return passive;
                    }
                }
            }
        }
        #endregion

        #region editor operations
        private bool ValidateEquipment(GearItemSlots slot)
        {
            if (slot.GetItem() != null)
                return slot.GetItem().GetSlotType().Equals(slot.slotType);
            return true;
        }
        #endregion

    }

    [System.Serializable]
    [InlineProperty(LabelWidth = 5)]
    public class GearItemSlots
    {
        [HideLabel]
        [SerializeField] GearItem item = null;
        [HideInInspector]
        public GearPiece slotType { get; private set; }

        public GearItemSlots(GearPiece slotType)
        {
            this.slotType = slotType;
        }

        public GearItemSlots()
        {

        }

        public bool SetItem(GearItem item)
        {
            if (item != null && item.GetSlotType() != slotType)
                return false;
            this.item = item;
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

