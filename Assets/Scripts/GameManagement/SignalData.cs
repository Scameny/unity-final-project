using CardSystem;
using Character.Stats;
using Character.Buff;
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
        CARD_USED,
        RECHARGE_DECK,
        DAMAGE_RECEIVED,
        RESOURCE_MODIFY,
        TURN_PREPARATION_START,
        NEW_TRAIT,
        TRAIT_RENEWED,
        CHARACTER_DIE,

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
        END_DRAGGING_CARD,
        CANCEL_TARGET_SELECTION,

        LEVEL_UP,
        MAX_RESOURCE_MODIFY,
        REMOVE_TRAIT,
        TRAIT_EXPIRED,
        TRAIT_MODIFIED,
        OUT_OF_COMBAT_CURRENT_RESOURCE_MODIFY,

        NONE,
        CARD_PLAYED,
        CARD_PLAYED_CANCEL,

        // error
        NOT_ENOUGH_RESOURCES,
        NOT_YOUR_TURN,
        NOT_ENOUGH_MONEY,


        DECK_INITIALIZE,
        SHUFFLE_DECK,
        CARD_CREATED,
        SEND_TO_STACK,
        CARD_EVOLVED,
        REMOVE_RESOURCE,
        ADD_RESOURCE,
        END_GAME
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

    public class CombatResourceSignalData : CombatSignalData
    {
        public ResourceType resourceType;
        public int resourceAmount;
        public int resourceBeforeGain;

        public CombatResourceSignalData(GameSignal signalType, GameObject user, IEnumerable<GameObject> charactersInCombat, ResourceType resourceType, int resourceAmount, int resourceBeforeGain) : base(signalType, user, charactersInCombat)
        {
            this.resourceType = resourceType;
            this.resourceAmount = resourceAmount;
            this.resourceBeforeGain = resourceBeforeGain;

        }
    }

    public class ResourceSignalData : SignalData
    {
        public ResourceType resourceType;
        public int newResourceMax;
        public int oldResourceMax;
        public GameObject user;

        public ResourceSignalData(GameSignal signalType, GameObject user, ResourceType resourceType, int newResourceMax, int oldResourceMax) : base(signalType)
        {
            this.resourceType = resourceType;
            this.newResourceMax = newResourceMax;
            this.oldResourceMax = oldResourceMax;
            this.user = user;

        }
    }

    public class TraitCombatSignalData : CombatSignalData
    {
        public BaseBuff trait;
        
        public TraitCombatSignalData(GameSignal signalType, GameObject user, IEnumerable<GameObject> charactersInCombat, BaseBuff trait) : base(signalType, user, charactersInCombat)
        {
            this.trait = trait;
        }
    }

    public class ModifyPrimaryStatisticSignalData : SignalData
    {
        public StatType statType;
        public GameObject user;

        public ModifyPrimaryStatisticSignalData(GameSignal signalType, GameObject user, StatType statType) : base(signalType)
        {
            this.statType = statType;
            this.user = user;
        }

    }

    public class ModifySecondaryStatisticSignalData : SignalData
    {
        public DamageTypeStat statType;
        public GameObject user;

        public ModifySecondaryStatisticSignalData(GameSignal signalType, GameObject user, DamageTypeStat statType) : base(signalType)
        {
            this.statType = statType;
            this.user = user;
    }
    }

    public class CardContainerSignalData : SignalData 
    {
        public ICardContainer container;
        public List<Card> cards;
        public GameObject user;

        public CardContainerSignalData(GameSignal signalType, GameObject user, ICardContainer container, IEnumerable<Card> cards) : base(signalType)
        {
            this.user = user;
            this.container = container;
            this.cards = new List<Card>(cards);
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

    public class ErrorSignalData : SignalData
    {
        public List<string> parameters;

        public ErrorSignalData(GameSignal signalType, List<string> parameters) : base(signalType)
        {
            this.parameters = parameters;
        }
    }

    public class EvolveCardSignalData : SignalData
    {
        public Card cardToSwap;
        public Usable swapCard;

        public EvolveCardSignalData(GameSignal signalType, Card cardToSwap, Usable swapCard) : base(signalType)
        {
            this.cardToSwap = cardToSwap;
            this.swapCard = swapCard;
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