using Character.Trait;
using Sirenix.OdinInspector;
using Strategies.PassiveEffectStrategies;
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
        [SerializeField] PassiveEffectStrategyList passiveEffectStrategiesList;

        #region Object operations

        public List<PassiveEffectStrategy> GetPassiveEffectStrategyList()
        {
            return passiveEffectStrategiesList.GetPassiveEffectStrategies();
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

    public struct PassiveData
    {
        public PassiveSignal signalType;
        public GameObject user;
        public IEnumerable<GameObject> targets;
    }

    public enum PassiveSignal
    {
        StartOfTurn,
        EndOfTurn,
        CardPlayed
    }

}
