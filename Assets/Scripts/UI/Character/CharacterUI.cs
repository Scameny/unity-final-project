using GameManagement;
using System;
using UnityEngine;

namespace UI.Character
{
    public class CharacterUI : MonoBehaviour, IObserver<SignalData>
    {

        GameObject selector;
        IDisposable disposable;


        virtual protected void Start()
        {
            selector = transform.Find("Selector").gameObject;
        }

        private void OnEnable()
        {
            disposable = UIManager.manager.Subscribe(this);
        }

        private void OnDisable()
        {
            disposable.Dispose();
        }

        public void EnableSelector(bool enable)
        {
            selector.SetActive(enable);
        }

        public void OnCompleted()
        {
            disposable.Dispose();
        }

        public void OnError(Exception error)
        {
            Debug.LogError("Character UI has an error:" + error.Message);
        }

        virtual public void OnNext(SignalData signalData)
        {

        }

    }
}
