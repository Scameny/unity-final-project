using GameManagement;
using System.Collections.Generic;
using UnityEngine;

namespace Strategies.SignalDecoderStrategy
{
    public abstract class SignalDecoderStrategy
    {

        [SerializeField] GameSignal passiveSignal;


        public abstract bool SignalEvaluate(List<SignalData> passiveDataStored, SignalData newSignal);
        
        public GameSignal GetPassiveSignal()
        {
            return passiveSignal;
        }

    }

}
