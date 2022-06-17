using Character.Character;
using Character.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Strategies.EffectStrategies
{
    public class ResourceGainEffect : EffectStrategy
    {
        public ResourceType resourceType;
        public int amount;

        public override void StartEffect(GameObject user, IEnumerable<GameObject> targets)
        {
            if (user.GetComponent<DefaultCharacter>().GetResourceType() == resourceType)
                user.GetComponent<DefaultCharacter>().GainResource(amount);
        }
    }
}

