using Character.Character;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Strategies.EffectStrategies
{
    [System.Serializable]
    public class FlatHealingEffect : HealEffectStrategy
    {
        [LabelWidth(120)]
        [SerializeField] public int healing;

        override protected void StartEffect(GameObject user, IEnumerable<GameObject> targets)
        {
            foreach (var target in targets)
            {
                DefaultCharacter character = target.GetComponent<DefaultCharacter>();
                user.GetComponent<DefaultCharacter>().Heal(healing);
            }
        }
    }
}

