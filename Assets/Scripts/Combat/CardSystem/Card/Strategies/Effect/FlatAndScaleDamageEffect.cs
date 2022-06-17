using Character.Stats;
using Character.Character;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Strategies.EffectStrategies
{

    public class FlatAndScaleDamageEffect : EffectStrategy
    {
        [HorizontalGroup]

        [LabelWidth(120)]
        public StatType stat;
        [HideLabel]
        [LabelText("Scale coeficient")]
        [LabelWidth(120)]
        public float scaleCoef;
        [LabelWidth(120)]
        public int flatDamage;
        [LabelWidth(120)]
        [SerializeField] public DamageType damageType;

        public override void StartEffect(GameObject user, IEnumerable<GameObject> targets)
        {
            float damageDone = user.GetComponent<DefaultCharacter>().GetStatistic(stat) * scaleCoef + flatDamage;
            damageDone = user.GetComponent<DefaultCharacter>().ProcessDamageDone(damageDone, damageType);
            foreach (var enemy in targets)
            {
                DefaultCharacter character = enemy.GetComponent<DefaultCharacter>();
                character.TakeDamage(damageDone, damageType);
            }
        }
    }
}
