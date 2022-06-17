using Abilities.Passive;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{ 
    public class PassiveManager : IObservable<PassiveData>
    {
        List<IObserver<PassiveData>> observers = new List<IObserver<PassiveData>>();

        public IDisposable Subscribe(IObserver<PassiveData> observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);
            return new Unsubscriber(observers, observer);
        }

        public void Unsubscribe()
        {
            foreach (var item in observers)
            {
                item.OnCompleted();
            }
        }

        public void SendData(PassiveSignal signal, GameObject user, IEnumerable<GameObject> targets)
        {
            PassiveData data;
            data.signalType = signal;
            data.user = user;
            data.targets = targets;
            foreach (var item in observers)
            {
                item.OnNext(data);
            }
        }

        private class Unsubscriber : IDisposable
        {
            private List<IObserver<PassiveData>> _observers;
            private IObserver<PassiveData> _observer;

            public Unsubscriber(List<IObserver<PassiveData>> observers, IObserver<PassiveData> observer)
            {
                _observers = observers;
                _observer = observer;
            }

            public void Dispose()
            {
                if (_observer != null && _observers.Contains(_observer))
                    _observers.Remove(_observer);
            }
        }
    }
}
