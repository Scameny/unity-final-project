using CardSystem;
using Character.Stats;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameManagement
{
    public enum GameSignal
    {
        START_COMBAT,
        END_COMBAT,
        ENTER_ROOM,
        START_TURN,
        END_TURN,
        CARD_DRAWED,
        CARD_PLAYED,
        DAMAGE_RECEIVED,
        RESOURCE_MODIFY
    }

    public class SignalData
    {
        public GameSignal signal { private set; get; }

        public SignalData(GameSignal signal)
        {
            this.signal = signal;
        }
    }

    public class CombatSignalData : SignalData
    {
        public GameObject user;
        public IEnumerable<GameObject> charactersInCombat;

        public CombatSignalData(GameSignal signalType, GameObject user, IEnumerable<GameObject> charactersInCombat) : base(signalType)
        {
            this.user = user;
            this.charactersInCombat = charactersInCombat;
        }
    }

    public class CombatCardSignalData : CombatSignalData
    {
        public Card card;

        public CombatCardSignalData(GameSignal signalType, GameObject user, IEnumerable<GameObject> targets, Card card) : base(signalType, user, targets)
        {
            this.card = card;
        }
    }

    public class DamageReceivedSignalData : CombatSignalData
    {
        public int damage;
        public DamageReceivedSignalData(GameSignal signalType, GameObject user, IEnumerable<GameObject> charactersInCombat, int damage) : base(signalType, user, charactersInCombat)
        {
            this.damage = damage;
        }
    }

    public class ResourceSignalData : CombatSignalData
    {
        public ResourceType resourceType;
        public int resourceAmount;
        public int resourceBeforeGain;

        public ResourceSignalData(GameSignal signalType, GameObject user, IEnumerable<GameObject> charactersInCombat, ResourceType resourceType, int resourceAmount, int resourceBeforeGain) : base(signalType, user, charactersInCombat)
        {
            this.resourceType = resourceType;
            this.resourceAmount = resourceAmount;
            this.resourceBeforeGain = resourceBeforeGain;

        }
    }

    public class Unsubscriber : IDisposable
    {
        private List<IObserver<SignalData>> _observers;
        private IObserver<SignalData> _observer;

        public Unsubscriber(List<IObserver<SignalData>> observers, IObserver<SignalData> observer)
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