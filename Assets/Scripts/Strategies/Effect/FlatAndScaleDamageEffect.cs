using Character.Stats;
using Character.Character;
using System.Collections.Generic;
using UnityEngine;

namespace Strategies.EffectStrategies
{
    [CreateAssetMenu(fileName = "FlatAndScaleDamageEffect", menuName = "Strategy/EffectStrategy/FlatAndScaleDamageEffect", order = 1)]
    public class FlatAndScaleDamageEffect : EffectStrategy
    {
        public StatType stat;
        public float scaleCoef;
        public int flatDamage;
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
