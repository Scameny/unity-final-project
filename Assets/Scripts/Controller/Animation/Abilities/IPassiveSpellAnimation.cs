using GameManagement;
using System.Collections.Generic;

namespace Animations
{
    public interface IPassiveSpellAnimation
    {
        public void PlaySpellAnimation(CombatSignalData passiveData, List<SignalData> signalDatas);
    }

}