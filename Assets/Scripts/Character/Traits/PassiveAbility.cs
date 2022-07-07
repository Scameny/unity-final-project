using CardSystem;
using Character.Stats;
using Sirenix.OdinInspector;
using Strategies.PassiveEffectStrategies;
using Strategies.SignalDecoderStrategy;
using System.Collections.Generic;
using UnityEngine;

namespace Abilities.Passive
{
    [CreateAssetMenu(fileName = "Passive", menuName = "Abilities/PassiveAbility", order = 1)]
    public class PassiveAbility : SerializedScriptableObject
    {
        [HorizontalGroup("Base", Width = 150)]

        [HideLabel, PreviewField(140)]
        [SerializeField]
        [VerticalGroup("Base/Left")]
        Sprite sprite;

        [SerializeField]
        [VerticalGroup("Base/Right")]
        string Name;

        [VerticalGroup("Base/Right")]
        [TextArea(4, 14)]
        [Space(4)]
        [SerializeField] string description;

        [InlineProperty, HideLabel]
        [SerializeReference] SignalDecoderStrategy signalDecoderStrategy;

        [InlineProperty, HideLabel]
        [SerializeField] PassiveEffectStrategyList passiveEffectStrategyList;

        #region Object operations

        public void Evaluate(List<PassiveData> passiveDataStored, PassiveData passiveData)
        {
            if (signalDecoderStrategy.SignalEvaluate(passiveDataStored, passiveData))
            {
                Debug.Log("Passive ability " + Name + " proced");
                foreach (var item in passiveEffectStrategyList.GetPassiveEffectStrategies())
                {
                    item.EffectActivation(passiveData);
                }
            }
        }

        public string GetDescription()
        {
            return description;
        }

        public string GetName()
        {
            return Name;
        }

        public Sprite GetSprite()
        {
            return sprite;
        }
        public override bool Equals(object obj)
        {
            return obj is PassiveAbility ability &&
                   base.Equals(obj) &&
                   Name == ability.Name;
        }

        public override int GetHashCode()
        {
            int hashCode = 890389916;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            return hashCode;
        }
        #endregion
    }


    public class PassiveData
    {
        public PassiveSignal signalType;
        public GameObject user;
        public IEnumerable<GameObject> targets;

        public PassiveData(PassiveSignal signalType, GameObject user, IEnumerable<GameObject> targets)
        {
            this.signalType = signalType;
            this.user = user;
            this.targets = targets;
        }
    }

    public class PassiveDataCardInteraction : PassiveData
    {
        public Card card;

        public PassiveDataCardInteraction(PassiveSignal signalType, GameObject user, IEnumerable<GameObject> targets, Card card) : base(signalType, user, targets)
        {
            this.card = card;
        }
    }

    public class PassiveDataResourceInteraction : PassiveData
    {
        public ResourceType resourceType;
        public int resourceAmount;
        public int resourceBeforeGain;

        public PassiveDataResourceInteraction(PassiveSignal signalType, GameObject user, IEnumerable<GameObject> targets, ResourceType resourceType, int resourceAmount, int resourceBeforeGain) : base(signalType, user, targets)
        {
            this.resourceType= resourceType;
            this.resourceAmount= resourceAmount;
            this.resourceBeforeGain= resourceBeforeGain;

        }
    }

    public enum PassiveSignal
    {
        StartOfTurn,
        EndOfTurn,
        CardDrawed,
        CardPlayed,
        DamageReceived,
        ResourceGained
    }

}
