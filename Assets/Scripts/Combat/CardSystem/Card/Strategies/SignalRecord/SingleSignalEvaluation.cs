using Abilities.Passive;
using System.Collections.Generic;

namespace Strategies.SignalDecoderStrategy
{
    public class SingleSignalEvaluation : SignalDecoderStrategy
    {
        public override bool SignalEvaluate(List<PassiveData> passiveDataStored, PassiveData newSignal)
        {
            if (newSignal.signalType.Equals(GetPassiveSignal()))
                return true;
            else
                return false;
        }
    }

}
