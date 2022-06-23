using CardSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager manager;
        public GameObject combatMenu, endTurnButton, dropZone;
        public GameObject characterMenu, inventory, openCharacterMenuButton, closeCharacterMenuButton;

        private void Awake()
        {
            manager = this;
        }

        public void ActivateCombatUI(bool enable)
        {
            combatMenu.SetActive(enable);
        }

        public void CombatUIInteractable(bool interactable)
        {
            endTurnButton.GetComponent<Button>().interactable = interactable;
            dropZone.SetActive(interactable);
        }


        public void ChangeSceneToSelection(IEnumerable<GameObject> targets, bool selection)
        {
            foreach(var character in targets)
            {
                character.GetComponentInChildren<CharacterUI>().EnableSelector(selection);
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