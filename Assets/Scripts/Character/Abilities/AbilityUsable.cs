using Character.Abilities;
using System.Collections.Generic;
using UnityEngine;

namespace Character.Abilities
{
    [System.Serializable]
    public class AbilityUsable
    {
        public Ability ability;
        public int currentSlots;

        public AbilityUsable(Ability ability)
        {
            this.ability = ability;
            currentSlots = ability.slot.slotsNumber;
        }

        public void RechargeSlots()
        {
            currentSlots = ability.slot.slotRecharge;
            currentSlots = Mathf.Max(currentSlots, ability.slot.slotsNumber);
        }

        public void UseAbility(GameObject user, IEnumerable<GameObject> targets)
        {
            if (!ability.hasSlots || currentSlots > 0)
            {
                ability.Use(user, targets);
                if (ability.hasSlots)
                    currentSlots -= 1;
            } 
            else
            {
                Debug.LogError("Not slots use for the ability " + ability.name);
            }
        }
    }
}

