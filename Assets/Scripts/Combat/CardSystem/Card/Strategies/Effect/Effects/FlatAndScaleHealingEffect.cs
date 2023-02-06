using Character.Character;
using Character.Stats;
using GameManagement;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Strategies.EffectStrategies
{
    public class FlatAndScaleHealingEffect : HealEffectStrategy
    {
        [HorizontalGroup]

        [LabelWidth(120)]
        [SerializeField] StatType stat;
        [HideLabel]
        [LabelText("Scale coeficient")]
        [LabelWidth(120)]
        [SerializeField] float scaleCoef;
        [LabelWidth(120)]
        [SerializeField] int flatHealing;


        public override int GetTotalHeal(GameObject user)
        {
            int healingDone = Mathf.FloorToInt(user.GetComponent<DefaultCharacter>().GetStatistic(stat) * scaleCoef) + flatHealing;
            return healingDone;
        }

        override protected List<SignalData> StartEffect(GameObject user, IEnumerable<GameObject> targets)
        {
            List<SignalData> signalDatas = new List<SignalData>();
            int healingDone = GetTotalHeal(user);
            foreach (var target in targets)
            {
                DefaultCharacter character = target.GetComponent<DefaultCharacter>();
                signalDatas.AddRange(character.Heal(healingDone));
            }
            return signalDatas;
        }
    }
}
