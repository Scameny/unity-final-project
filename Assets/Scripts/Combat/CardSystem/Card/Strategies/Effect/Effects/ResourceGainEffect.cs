using Character.Character;
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
            user.GetComponent<DefaultCharacter>().GainResource(amount, resourceType);
        }
    }
}

