using GameManagement;
using System;

namespace Animations
{
    public interface IPassiveSpellAnimation
    {
        public void PlaySpellAnimation(CombatSignalData passiveData, Action<CombatSignalData> function);
    }

}