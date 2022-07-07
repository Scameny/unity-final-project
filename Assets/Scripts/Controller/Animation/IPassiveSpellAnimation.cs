using Abilities.Passive;
using System;

namespace Animations
{
    public interface IPassiveSpellAnimation
    {
        public void PlaySpellAnimation(PassiveData passiveData, Action<PassiveData> function);
    }

}