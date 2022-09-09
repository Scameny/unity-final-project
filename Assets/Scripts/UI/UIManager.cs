using GameManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using UI.Character;
using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour, IObservable<SignalData>
    {

        private List<IObserver<SignalData>> observers = new List<IObserver<SignalData>>();

        public static UIManager manager;

        SimpleTooltipStyle tooltipStyle;

        private void Awake()
        {
            manager = this;
        }

        private void Start()
        {
            tooltipStyle = Resources.Load<SimpleTooltipStyle>("UI/TooltipStyle");
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
            foreach (var item in observers.ToList())
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

        public SimpleTooltipStyle GetTooltipStyle()
        {
            return tooltipStyle;
        }
    }
}