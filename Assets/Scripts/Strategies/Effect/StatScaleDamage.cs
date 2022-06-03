using System.Collections.Generic;
using UnityEngine;
using Character.Stats;
using Character.Character;

namespace Strategies.EffectStrategies
{ 
    [CreateAssetMenu(fileName = "StatScaleDamage", menuName = "Strategy/EffectStrategy/StatScaleDamage", order = 2)]
    public class StatScaleDamage : EffectStrategy
    {
        public StatType stat;
        public float scaleCoef;
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
