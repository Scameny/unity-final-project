using Character.Character;
using Character.Stats;
using GameManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Strategies.SignalDecoderStrategy
{
    public class ResourceModifySignalDecoder : SignalDecoderStrategy
    {
        [SerializeField] ResourceType resourceType;
        [SerializeField] int quantity;

        public override bool SignalEvaluate(List<SignalData> passiveDataStored, SignalData newSignal)
        {
            if (newSignal.signal.Equals(GameSignal.RESOURCE_MODIFY))
            {
                CombatResourceSignalData data = newSignal as CombatResourceSignalData;
                if (data.resourceType.Equals(resourceType) && data.user.GetComponent<DefaultCharacter>().GetCurrentResource(resourceType) == quantity)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
