using Character.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Strategies.EffectStrategies
{
    [CreateAssetMenu(fileName = "FlatHealingEffect", menuName = "Strategy/EffectStrategy/FlatHealingEffect", order = 3)]
    public class FlatHealingEffect : EffectStrategy
    {
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

