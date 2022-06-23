using System.Collections.Generic;
using UnityEngine;
using Character.Stats;
using Character.Character;
using Sirenix.OdinInspector;

namespace Strategies.EffectStrategies
{ 
    [System.Serializable]
    public class StatScaleDamage : EffectStrategy
    {
        [LabelWidth(120)]
        public StatType stat;
        [LabelWidth(120)]
        public float scaleCoef;
        [LabelWidth(120)]
        [SerializeField] public DamageType damageType;

        override public void StartEffect(GameObject user, IEnumerable<GameObject> targets)
        {
            int damageDone = Mathf.FloorToInt(user.GetComponent<DefaultCharacter>().GetStatistic(stat) * scaleCoef);
            damageDone = user.GetComponent<DefaultCharacter>().ProcessDamageDone(damageDone, damageType);
            foreach (var enemy in targets)
            {
                DefaultCharacter character = enemy.GetComponent<DefaultCharacter>();
                character.TakeDamage(damageDone, damageType, user);
            }
        }
    }
}
