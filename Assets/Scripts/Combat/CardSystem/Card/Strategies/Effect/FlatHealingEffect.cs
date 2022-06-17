using Character.Character;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Strategies.EffectStrategies
{
    public class FlatHealingEffect : EffectStrategy
    {
        [LabelWidth(120)]
        [SerializeField] public float healing;

        public override void StartEffect(GameObject user, IEnumerable<GameObject> targets)
        {
            foreach (var target in targets)
            {
                DefaultCharacter character = target.GetComponent<DefaultCharacter>();
                user.GetComponent<DefaultCharacter>().Heal(healing);
            }
        }
    }
}

