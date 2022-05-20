using Combat;
using Character.Abilities;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class AbilityButton : MonoBehaviour
    {
        public AbilityUsable ability;

        public void UseAbility(HeroCombat player)
        {
            player.UseAbility(ability);
            transform.parent.gameObject.SetActive(false);
            if (ability.ability.hasSlots && ability.currentSlots == 0)
            {
                GetComponent<Button>().enabled = false;
            }
        }
    }
}

