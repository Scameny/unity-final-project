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
        public GameObject combatMenu, endTurnButton, dropZone, resourcesMenu;
        public GameObject characterMenu, inventory, openCharacterMenuButton, closeCharacterMenuButton;

        public SimpleTooltipStyle tooltipStyle;

        private void Awake()
        {
            manager = this;
        }

        private void Start()
        {
            tooltipStyle = Resources.Load<SimpleTooltipStyle>("UI/TooltipStyle");
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
            resourcesMenu.SetActive(!enable);
        }

    }
}