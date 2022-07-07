using Abilities.Passive;
using CardSystem;
using System.Collections.Generic;
using UnityEngine;

namespace Strategies.SignalDecoderStrategy
{
    public class SpecificCardsTypeUsedSignalEvaluation : SignalDecoderStrategy
    {
        [SerializeField] List<AbilityType> abilityTypes = new List<AbilityType>();

        public override bool SignalEvaluate(List<PassiveData> passiveDataStored, PassiveData newSignal)
        {
            if (newSignal.signalType.Equals(GetPassiveSignal()))
            {
                List<AbilityType> abilityTypesCovered = new List<AbilityType>(abilityTypes);
                foreach (var item in passiveDataStored)
                {
                    abilityTypesCovered.Remove((item as PassiveDataCardInteraction).card.GetUsable().GetAbilityType());
                }
                if (abilityTypesCovered.Remove((newSignal as PassiveDataCardInteraction).card.GetUsable().GetAbilityType()))
                {
                    if (abilityTypesCovered.Count == 0)
                    {
                        passiveDataStored.Clear();
                        return true;
                    }   
                    else
                        passiveDataStored.Add(newSignal);
                }
            }
            return false;
        }
    }
}
