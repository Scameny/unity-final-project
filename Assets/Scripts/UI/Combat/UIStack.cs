using GameManagement;
using System;
using UnityEngine;

namespace UI.Combat
{
    public class UIStack : MonoBehaviour, IObserver<SignalData>
    {

        IDisposable disposable;
        GameObject character;

        private void Start()
        {
            disposable = UIManager.manager.Subscribe(this);
            character = GameObject.FindGameObjectWithTag("Player");
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
            if ((value.signal.Equals(GameSignal.SEND_TO_STACK)) && character.Equals((value as CombatCardSignalData).user))
            {
                CombatCardSignalData data = value as CombatCardSignalData;
                data.card.SetVisibility(false);
                data.card.transform.SetParent(transform);
            }
        }

    }

}
