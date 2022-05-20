using System;
using System.Collections.Generic;
using UnityEngine;
using Combat;
using Character.Character;

namespace Strategies.EffectStrategies
{
    [CreateAssetMenu(fileName = "FlatDamageEffect", menuName = "Strategy/EffectStrategy/FlatDamageEffect", order = 1)]
    public class FlatDamageEffect : EffectStrategy
    {
        [SerializeField] public float damage;
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

