using GameManagement;
using System;
using UnityEngine;

namespace UI
{
    public class UIResourcesFrame : MonoBehaviour, IObserver<SignalData>
    {
        IDisposable disposable;

        private void Start()
        {
            disposable = UIManager.manager.Subscribe(this);
        }

        public void OnCompleted()
        {
            disposable.Dispose();
        }

        public void OnError(Exception error)
        {
            Debug.LogError("Error on resources frame");
        }

        public void OnNext(SignalData value)
        {
            if (value.signal.Equals(GameSignal.START_INTERACTION))
            {
                gameObject.SetActive(false);
            }
            else if (value.signal.Equals(GameSignal.END_INTERACTION) || value.signal.Equals(GameSignal.START_GAME))
            {
                gameObject.SetActive(true);
            }
        }

    }

}
