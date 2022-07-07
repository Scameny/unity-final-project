using Abilities.Passive;
using System;
using System.Collections.Generic;

namespace Combat
{ 
    public class PassiveManager : IObservable<PassiveData>
    {
        private List<IObserver<PassiveData>> observers;

        public PassiveManager()
        {
            observers = new List<IObserver<PassiveData>>();
        }

        public IDisposable Subscribe(IObserver<PassiveData> observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);
            return new Unsubscriber(observers, observer);
        }

        public void Unsubscribe()
        {
            while (observers.Count > 0)
            {
                observers[0].OnCompleted();
            }
        }

        public void SendData(PassiveData passiveData)
        {
            foreach (var item in observers)
            {
                item.OnNext(passiveData);
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
