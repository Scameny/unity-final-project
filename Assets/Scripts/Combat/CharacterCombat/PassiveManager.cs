using GameManagement;
using System;
using System.Collections.Generic;

namespace Combat
{ 
    public class PassiveManager : IObservable<SignalData>
    {
        private List<IObserver<SignalData>> observers;

        public PassiveManager()
        {
            observers = new List<IObserver<SignalData>>();
        }

        public IDisposable Subscribe(IObserver<SignalData> observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);
            return new Unsubscriber(observers, observer);
        }

        public void Unsubscribe()
        {
            for (int i = 0; i < observers.Count; i++)
            {
                observers[i].OnCompleted();
            }
        }

        public void SendData(SignalData passiveData)
        {
            foreach (var item in observers)
            {
                item.OnNext(passiveData);
            }
        }
    }
}
