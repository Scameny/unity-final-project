using Character.Character;
using Character.Trait;
using System.Collections.Generic;
using UnityEngine;

namespace Strategies.EffectStrategies
{
    [CreateAssetMenu(fileName = "ApplyBuffEffect", menuName = "Strategy/EffectStrategy/ApplyBuffEffect", order = 1)]
    public class ApplyBuffEffect : EffectStrategy
    {
        public BaseTrait trait;

        public override void StartEffect(GameObject user, IEnumerable<GameObject> targets)
        {
            foreach (var target in targets)
            {
                DefaultCharacter character = target.GetComponent<DefaultCharacter>();
                character.AddNewTrait(trait);
            }
        }
    }
}
