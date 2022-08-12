using CardSystem;
using Character.Stats;
using Interaction;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameManagement
{
    public enum GameSignal
    {
        // COMBAT SIGNALS
        START_COMBAT,
        END_COMBAT,
        START_TURN,
        END_TURN,
        CARD_DRAWED,
        CARD_PLAYED,
        DAMAGE_RECEIVED,
        RESOURCE_MODIFY,
        TURN_PREPARATION_START,

        // GAME SIGNALS
        START_GAME,
        ENTER_ROOM,
        PRIMARY_STAT_MODIFY,
        SECONDARY_STAT_MODIFY,
        END_INTERACTION,
        START_INTERACTION,


        // UI SPECIFIC SIGNALS
        ENABLE_UI_ELEMENT,
        ASSIGN_NPC_UI_ELEMENT,
        OPEN_CHARACTER_MENU,
        CLOSE_CHARACTER_MENU,
        START_DRAGGING_CARD,
        END_DRAGGING_CARD
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

    public abstract class ModifyStatisticSignalData : SignalData
    {
        public GameObject user;
        public int amount;
        public int statBeforeGain;

        public ModifyStatisticSignalData(GameSignal signalType, GameObject user, int amount, int statBeforeGain) : base(signalType)
        {
            this.user = user;
            this.amount = amount;
            this.statBeforeGain = statBeforeGain;
        }
    }

    public class ModifyPrimaryStatisticSignalData : ModifyStatisticSignalData
    {
        public StatType statType;

        public ModifyPrimaryStatisticSignalData(GameSignal signalType, GameObject user, StatType statType, int amount, int statBeforeGain) : base(signalType, user, amount, statBeforeGain)
        {
            this.statType = statType;
        }

    }

    public class ModifySecondaryStatisticSignalData : ModifyStatisticSignalData
    {
        public DamageTypeStat statType;

        public ModifySecondaryStatisticSignalData(GameSignal signalType, GameObject user, DamageTypeStat statType, int amount, int statBeforeGain) : base(signalType, user, amount, statBeforeGain)
        {
            this.statType = statType;
        }
    }

    public class UISignalData : SignalData
    {
        public UIElement element;
        public bool enable;

        public UISignalData(GameSignal signalType, UIElement element, bool enable) : base(signalType)
        {
            this.element = element;
            this.enable = enable;
        }
    }

    public class UINpcSignalData : UISignalData
    {
        public NPCInteractable npc;

        public UINpcSignalData(GameSignal signalType, UIElement element, bool enable, NPCInteractable npc) : base(signalType, element, enable)
        {
            this.element = element;
            this.npc = npc;
        }
    }

    public enum UIElement
    {
        RESOURCES_FRAME,
        MENU_FRAME,
        REMOVE_CARD_FRAME,
        VENDOR_FRAME,
        COMBAT_FRAME
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