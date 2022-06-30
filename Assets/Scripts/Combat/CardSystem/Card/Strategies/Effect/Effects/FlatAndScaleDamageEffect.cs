using Character.Stats;
using Character.Character;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

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

        override protected void StartEffect(GameObject user, IEnumerable<GameObject> targets)
        {
            int damageDone = GetTotalDamage(user);
            foreach (var enemy in targets)
            {
                DefaultCharacter character = enemy.GetComponent<DefaultCharacter>();
                character.TakeDamage(damageDone, damageType);
            }
        }
    }
}
