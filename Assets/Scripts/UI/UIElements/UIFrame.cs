using GameManagement;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI.UIElements 
{
    public class UIFrame : MonoBehaviour, IObserver<SignalData>
    {
        [SerializeField] List<GameSignal> activeSignals;
        [SerializeField] List<GameSignal> desactivateSignals;
        IDisposable disposable;

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

        void Start()
        {
            disposable = UIManager.manager.Subscribe(this);
        }

    }
}
