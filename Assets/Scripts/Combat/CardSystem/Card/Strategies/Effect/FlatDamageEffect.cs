using System.Collections.Generic;
using UnityEngine;
using Character.Character;
using Sirenix.OdinInspector;

namespace Strategies.EffectStrategies
{
    public class FlatDamageEffect : EffectStrategy
    {
        [LabelWidth(120)]
        [SerializeField] public float damage;
        [LabelWidth(120)]
        [SerializeField] public DamageType damageType;

        public override void StartEffect(GameObject user, IEnumerable<GameObject> targets)
        {
            float totalDamage = user.GetComponent<DefaultCharacter>().ProcessDamageDone(damage, damageType);
            foreach (var target in targets)
            {
                DefaultCharacter character = target.GetComponent<DefaultCharacter>();
                character.TakeDamage(totalDamage, damageType);
            }
        }
    }
}

