using CardSystem;
using GameManagement;
using System.Collections.Generic;
using UnityEngine;

namespace Strategies.SignalDecoderStrategy
{
    public class SpecificCardsTypeUsedSignalEvaluation : SignalDecoderStrategy
    {
        [SerializeField] List<AbilityType> abilityTypes = new List<AbilityType>();

        public override bool SignalEvaluate(List<SignalData> passiveDataStored, SignalData newSignal)
        {
            if (newSignal.signal.Equals(GetPassiveSignal()))
            {
                List<AbilityType> abilityTypesCovered = new List<AbilityType>(abilityTypes);
                foreach (var item in passiveDataStored)
                {
                    abilityTypesCovered.Remove((item as CombatCardSignalData).card.GetUsable().GetAbilityType());
                }
                if (abilityTypesCovered.Remove((newSignal as CombatCardSignalData).card.GetUsable().GetAbilityType()))
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
