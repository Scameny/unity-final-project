using GameManagement;
using System.Collections.Generic;

namespace Strategies.SignalDecoderStrategy
{
    public class SingleSignalEvaluation : SignalDecoderStrategy
    {
        public override bool SignalEvaluate(List<SignalData> passiveDataStored, SignalData newSignal)
        {
            if (newSignal.signal.Equals(GetPassiveSignal()))
                return true;
            else
                return false;
        }
    }

}
