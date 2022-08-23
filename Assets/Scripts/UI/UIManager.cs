using GameManagement;
using System;
using System.Collections.Generic;
using UnityEngine;

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

        public void ChangeSceneToSelection(IEnumerable<GameObject> targets, bool selection)
        {
            foreach (var character in targets)
            {
                character.GetComponentInChildren<CharacterUI>().EnableSelector(selection);
            }
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

        public void SendData(List<SignalData> listOfUIData)
        {
            foreach (var item in listOfUIData)
            {
                SendData(item);
            }
        }
    }
}