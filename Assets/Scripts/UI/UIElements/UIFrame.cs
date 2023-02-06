using GameManagement;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI.UIElements 
{
    public class UIFrame : MonoBehaviour, IObserver<SignalData>
    {
        [SerializeField] List<GameSignal> activeSignals;
        [SerializeField] List<GameSignal> deactivateSignals;
        IDisposable disposable;

        public void OnCompleted()
        {
            disposable.Dispose();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        virtual public void OnNext(SignalData value)
        {
            if (deactivateSignals.Exists(s => s.Equals(value.signal)))
            {
                gameObject.SetActive(false);
            }
            else if (activeSignals.Exists(s => s.Equals(value.signal)))
            {
                gameObject.SetActive(true);
            }
        }

        void Awake()
        {
            disposable = UIManager.manager.Subscribe(this);
        }

        #region getters and setters
        public List<GameSignal> GetActiveSignals()
        {
            return activeSignals;
        }

        public List<GameSignal> GetDeactiveSignals()
        {
            return deactivateSignals;
        }

        #endregion

    }
}
