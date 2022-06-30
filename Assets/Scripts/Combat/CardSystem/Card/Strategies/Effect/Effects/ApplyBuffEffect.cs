using Character.Character;
using Character.Trait;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Strategies.EffectStrategies
{
    [System.Serializable]
    public class ApplyBuffEffect : BuffEffectStrategy
    {
        [LabelWidth(120)]
        public BaseTrait trait;

        override protected void StartEffect(GameObject user, IEnumerable<GameObject> targets)
        {
            foreach (var target in targets)
            {
                DefaultCharacter character = target.GetComponent<DefaultCharacter>();
                character.AddNewTrait(trait);
            }
        }
    }
}
