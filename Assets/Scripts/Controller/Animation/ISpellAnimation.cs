using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animations
{
    public interface ISpellAnimation
    {
        public void PlaySpellAnimation(GameObject user, IEnumerable<GameObject> targets, Action<GameObject, IEnumerable<GameObject>> function);
    }
}
