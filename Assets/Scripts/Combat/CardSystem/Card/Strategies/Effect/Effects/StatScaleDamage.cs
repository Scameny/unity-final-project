using System.Collections.Generic;
using UnityEngine;
using Character.Stats;
using Character.Character;
using Sirenix.OdinInspector;
using GameManagement;

namespace Strategies.EffectStrategies
{ 
    [System.Serializable]
    public class StatScaleDamage : DamageEffectStrategy
    {
        [LabelWidth(120)]
        [SerializeField] StatType stat;
        [LabelWidth(120)]
        [SerializeField] float scaleCoef;
        [LabelWidth(120)]
        [SerializeField] public DamageType damageType;

        public override int GetTotalDamage(GameObject user)
        {
            int damageDone = Mathf.FloorToInt(user.GetComponent<DefaultCharacter>().GetStatistic(stat) * scaleCoef);
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
