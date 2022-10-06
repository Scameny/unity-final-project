using Character.Character;
using GameManagement;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Strategies.EffectStrategies
{
    [System.Serializable]
    public class FlatHealingEffect : HealEffectStrategy
    {
        [LabelWidth(120)]
        [SerializeField] public int healing;

        public override int GetTotalHeal(GameObject user)
        {
            return healing;
        }

        override protected List<SignalData> StartEffect(GameObject user, IEnumerable<GameObject> targets)
        {
            List<SignalData> signalDatas = new List<SignalData>();
            foreach (var target in targets)
            {
                DefaultCharacter character = target.GetComponent<DefaultCharacter>();
                signalDatas.AddRange(user.GetComponent<DefaultCharacter>().Heal(healing));
            }
            return signalDatas;
        }
    }
}

