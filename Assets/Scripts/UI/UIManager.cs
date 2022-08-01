using GameManagement;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIManager : MonoBehaviour, IObservable<SignalData>
    {

        private List<IObserver<SignalData>> observers = new List<IObserver<SignalData>>();

        public static UIManager manager;
        public GameObject combatMenu, endTurnButton, dropZone, resourcesMenu, permanentCardsRemoveWindow, vendorFrame;
        public GameObject characterMenu, inventory, openCharacterMenuButton, closeCharacterMenuButton, conversationFrame;

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
            foreach (var character in targets)
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

        public void NPCInteraction(bool enable)
        {
            openCharacterMenuButton.SetActive(!enable);
            resourcesMenu.SetActive(!enable);
            conversationFrame.SetActive(enable);
        }

        public void EnablePermanentCardsRemoveWindow(bool enable)
        {
            permanentCardsRemoveWindow.SetActive(enable);
        }

        public void EnableVendorFrame(bool enable)
        {
            vendorFrame.SetActive(enable);
        }

        public GameObject GetVendorFrame()
        {
            return vendorFrame;
        }

        public GameObject GetPermanentCardsRemoveWindow()
        {
            return permanentCardsRemoveWindow;
        }

        public IDisposable Subscribe(IObserver<SignalData> observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);
            return new Unsubscriber(observers, observer);
        }

        public void SendData(SignalData uiData)
        {
            foreach (var item in observers)
            {
                item.OnNext(uiData);
            }
        }
    }
}