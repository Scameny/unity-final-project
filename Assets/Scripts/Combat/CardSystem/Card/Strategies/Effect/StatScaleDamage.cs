using System.Collections.Generic;
using UnityEngine;
using Character.Stats;
using Character.Character;
using Sirenix.OdinInspector;

namespace Strategies.EffectStrategies
{ 
    public class StatScaleDamage : EffectStrategy
    {
        [LabelWidth(120)]
        public StatType stat;
        [LabelWidth(120)]
        public float scaleCoef;
        [LabelWidth(120)]
        [SerializeField] public DamageType damageType;

        public override void StartEffect(GameObject user, IEnumerable<GameObject> targets)
        {
            float damageDone = user.GetComponent<DefaultCharacter>().GetStatistic(stat) * scaleCoef;
            damageDone = user.GetComponent<DefaultCharacter>().ProcessDamageDone(damageDone, damageType);
            foreach (var enemy in targets)
            {
                DefaultCharacter character = enemy.GetComponent<DefaultCharacter>();
                character.TakeDamage(damageDone, damageType);
            }
        }
    }
}
