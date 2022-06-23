using Character.Character;
using Character.Stats;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Strategies.EffectStrategies
{
    [System.Serializable]
    public class ResourceGainEffect : EffectStrategy
    {
        [LabelWidth(120)]
        public ResourceType resourceType;
        [LabelWidth(120)]
        public int amount;

        override public void StartEffect(GameObject user, IEnumerable<GameObject> targets)
        {
            if (user.GetComponent<DefaultCharacter>().GetResourceType().Contains(resourceType))
                user.GetComponent<DefaultCharacter>().GainResource(amount, resourceType);
        }
    }
}

