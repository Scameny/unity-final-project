using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character.Stats 
{
    public interface IModifierProvider
    {
        IEnumerable<float> GetAdditiveModifier(StatType stat);
    
    }
}
