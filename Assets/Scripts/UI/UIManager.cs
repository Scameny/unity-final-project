using Combat;
using Character.Abilities;
using Items;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager manager;
        public GameObject combatMenu, abilityMenu, itemMenu, itemButton, abilityButton;

        private void Awake()
        {
            manager = this;
        }

        public void ActivateCombatMenu()
        {
            combatMenu.SetActive(true);
        }
        
        public void PopulateAbilityMenu(List<AbilityUsable> abilities, HeroCombat player)
        {

            foreach (var abilityUsable in abilities)
            {
                GameObject abilitySlot = Instantiate(abilityButton, abilityMenu.transform);
                abilitySlot.GetComponentInChildren<Text>().text = abilityUsable.ability.name;
                abilitySlot.GetComponent<AbilityButton>().ability = abilityUsable;
                abilitySlot.GetComponent<Button>().onClick.AddListener(() => { abilitySlot.GetComponent<AbilityButton>().UseAbility(player); });
            }
        }

        public void PopulateItemMenu(List<Item> items, HeroCombat player)
        {
            foreach (var usableItem in items)
            {
                GameObject itemSlot = Instantiate(itemButton, itemMenu.transform);
                itemSlot.GetComponent<ItemButton>().item = usableItem;
                itemSlot.GetComponentInChildren<Text>().text = usableItem.name;
                itemSlot.GetComponent<Button>().onClick.AddListener(() => { itemSlot.GetComponent<ItemButton>().UseItem(player); });
            }
        }


        public void EnableSelectorInTargets(IEnumerable<GameObject> targets, bool enable)
        {
            foreach(var character in targets)
            {
                character.GetComponent<TurnCombat>().selector.SetActive(enable);
            }
        }

    }

}