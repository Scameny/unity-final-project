using GameManagement;
using System.Collections.Generic;
using UnityEngine;

namespace Animations
{
    public interface ISpellAnimation
    {
        public void PlaySpellAnimation(GameObject user, IEnumerable<GameObject> targets, List<SignalData> uiSignals);
    }
}
