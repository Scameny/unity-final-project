using Character.Stats;  
using UnityEngine;

namespace Items
{
    [System.Serializable]
    public class Gear
    {

        public GearItemSlots head = new GearItemSlots(GearPiece.Helm);
        public GearItemSlots chest = new GearItemSlots(GearPiece.Chest);
        public GearItemSlots legs = new GearItemSlots(GearPiece.Legs);
        public GearItemSlots weapon = new GearItemSlots(GearPiece.Weapon);
        public GearItemSlots gloves = new GearItemSlots(GearPiece.Gloves);
        public GearItemSlots firstRing = new GearItemSlots(GearPiece.Ring);
        public GearItemSlots secondRing=  new GearItemSlots(GearPiece.Ring);

        public float GetAdditiveModifier(StatType stat)
        {
            GearItemSlots[] items = new GearItemSlots[7] { head, chest, legs, weapon, gloves, firstRing, secondRing };
            float value = 0;
            foreach (var slot in items)
            {
                if (slot.item != null) { 
                    foreach (var statValue in slot.item.GetAdditiveModifier(stat))
                    {
                         value += statValue;
                    }
                }
            }
            return value;
        }

        public float GetAdditiveModifier(DamageTypeStat stat)
        {
            GearItemSlots[] items = new GearItemSlots[7] { head, chest, legs, weapon, gloves, firstRing, secondRing };
            float value = 0;
            foreach (var slot in items)
            {
                if (slot.item != null)
                {
                    foreach (var statValue in slot.item.GetAdditiveModifier(stat))
                    {
                        value += statValue;
                    }
                }
            }
            return value;
        }




    }

    [System.Serializable]
    public class GearItemSlots
    {
        public GearItemSlots(GearPiece slotType)
        {
            this.slotType = slotType;
        }
        public GearItem item = null;
        [HideInInspector]
        public GearPiece slotType { get; private set; }

        public bool SetItem(GearItem item)
        {
            if (item.slot == slotType)
            {
                this.item = item;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

