using GameManagement;
using System.Collections.Generic;
using UnityEngine;

namespace Character.Stats 
{
    public interface IModifierProvider
    {
        IEnumerable<int> GetAdditiveModifier(StatType stat);

        IEnumerable<int> GetAdditiveModifier(DamageTypeStat stat);

        List<SignalData> GetSignalDatas(GameObject user);
    }


}
