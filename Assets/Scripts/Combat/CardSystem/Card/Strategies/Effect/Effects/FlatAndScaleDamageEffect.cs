using Character.Stats;
using Character.Character;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using GameManagement;

namespace Strategies.EffectStrategies
{
    [System.Serializable]
    public class FlatAndScaleDamageEffect : DamageEffectStrategy
    {
        [HorizontalGroup]

        [LabelWidth(120)]
        [SerializeField] StatType stat;
        [HideLabel]
        [LabelText("Scale coeficient")]
        [LabelWidth(120)]
        [SerializeField] float scaleCoef;
        [LabelWidth(120)]
        [SerializeField] int flatDamage;
        [LabelWidth(120)]
        [SerializeField] DamageType damageType;

        public override int GetTotalDamage(GameObject user)
        {
            int damageDone = Mathf.FloorToInt(user.GetComponent<DefaultCharacter>().GetStatistic(stat) * scaleCoef) + flatDamage;
            damageDone = user.GetComponent<DefaultCharacter>().ProcessDamageDone(damageDone, damageType);
            return damageDone;
        }

        override protected List<SignalData> StartEffect(GameObject user, IEnumerable<GameObject> targets)
        {
            List<SignalData> signalDatas = new List<SignalData>();
            int damageDone = GetTotalDamage(user);
            foreach (var enemy in targets)
            {
                DefaultCharacter character = enemy.GetComponent<DefaultCharacter>();
                signalDatas.AddRange(character.TakeDamage(damageDone, damageType));
            }
            return signalDatas;
        }
    }
}
