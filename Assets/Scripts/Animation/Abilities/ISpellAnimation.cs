using GameManagement;
using System.Collections.Generic;
using UnityEngine;

namespace Animations.Ability
{
    public interface ISpellAnimation
    {
        public void PlaySpellAnimation(GameObject user, IEnumerable<GameObject> targets, List<SignalData> uiSignals);
    }
}
