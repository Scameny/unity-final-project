using Character.Character;
using GameManagement;
using Sirenix.OdinInspector;
using System.Collections.Generic;
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

        override protected List<SignalData> StartEffect(GameObject user, IEnumerable<GameObject> targets)
        {
            return user.GetComponent<DefaultCharacter>().GainResource(amount, resourceType);
        }
    }
}

