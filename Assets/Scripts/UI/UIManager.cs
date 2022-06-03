using Combat;
using Items;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager manager;
        public GameObject combatMenu;
        public GameObject characterMenu, inventory, openCharacterMenuButton, closeCharacterMenuButton;

        private void Awake()
        {
            manager = this;
        }

        public void ActivateCombatUI(bool enable)
        {
            Debug.Log("Combat UI enabled: " + enable);
            combatMenu.SetActive(enable);
        }


        public void ChangeSceneToSelection(IEnumerable<GameObject> targets, bool selection)
        {
            foreach(var character in targets)
            {
                character.GetComponent<TurnCombat>().selector.SetActive(selection);
            }
        }

        public void ActiveCharacterMenu(bool enable)
        {
            characterMenu.SetActive(enable);
            inventory.SetActive(enable);
            openCharacterMenuButton.SetActive(!enable);
            closeCharacterMenuButton.SetActive(enable);
        }

    }

}