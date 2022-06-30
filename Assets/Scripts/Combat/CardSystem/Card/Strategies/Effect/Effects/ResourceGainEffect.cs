using Character.Character;
using Character.Stats;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Strategies.EffectStrategies
{
    [System.Serializable]
    public class ResourceGainEffect : ResourceGainEffectStrategy
    {
        [LabelWidth(120)]
        [SerializeField] int amount;

        public override int GetResourceAmountGained()
        {
            return amount;
        }

        override protected void StartEffect(GameObject user, IEnumerable<GameObject> targets)
        {
            if (user.GetComponent<DefaultCharacter>().GetResourceType().Contains(resourceType))
                user.GetComponent<DefaultCharacter>().GainResource(amount, resourceType);
        }
    }
}

