using Abilities.Passive;
using System.Collections.Generic;
using UnityEngine;

namespace Strategies.SignalDecoderStrategy
{
    public abstract class SignalDecoderStrategy
    {

        [SerializeField] PassiveSignal passiveSignal;


        public abstract bool SignalEvaluate(List<PassiveData> passiveDataStored, PassiveData newSignal);
        
        public PassiveSignal GetPassiveSignal()
        {
            return passiveSignal;
        }

    }

}
