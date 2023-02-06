using GameManagement;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class UIButton : MonoBehaviour, IObserver<SignalData>
    {
        [SerializeField] GameSignal signalSentOnClick;
        [SerializeField] List<GameSignal> activeSignals;
        [SerializeField] List<GameSignal> desactivateSignals;
        IDisposable disposable;

        private void Start()
        {
            if (activeSignals.Count > 0  || desactivateSignals.Count > 0)
                UIManager.manager.Subscribe(this);
        }

        public void OnClick()
        {
            UIManager.manager.SendData(new SignalData(signalSentOnClick));    
        }

        public void OnCompleted()
        {
            disposable.Dispose();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(SignalData value)
        {
            if (desactivateSignals.Exists(s => s.Equals(value.signal)))
            {
                gameObject.SetActive(false);
            } 
            else if (activeSignals.Exists(s => s.Equals(value.signal)))
            {
                gameObject.SetActive(true);
            }
        }
    }
}
