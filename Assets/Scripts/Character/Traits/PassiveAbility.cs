using GameManagement;
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

        public void Evaluate(List<SignalData> passiveDataStored, SignalData passiveData)
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

}
