using Character.Character;
using Character.Buff;
using GameManagement;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Strategies.EffectStrategies
{
    [System.Serializable]
    public class ApplyBuffEffect : BuffEffectStrategy
    {
        [LabelWidth(120)]
        public BaseBuff trait;

        override protected List<SignalData> StartEffect(GameObject user, IEnumerable<GameObject> targets)
        {
            List<SignalData> signalDatas = new List<SignalData>();
            foreach (var target in targets)
            {
                DefaultCharacter character = target.GetComponent<DefaultCharacter>();
                signalDatas.AddRange(character.AddNewTrait(trait));
            }
            return signalDatas;
        }
    }
}
