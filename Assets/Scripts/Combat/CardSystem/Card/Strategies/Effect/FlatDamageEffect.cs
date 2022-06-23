using System.Collections.Generic;
using UnityEngine;
using Character.Character;
using Sirenix.OdinInspector;

namespace Strategies.EffectStrategies
{
    [System.Serializable]
    public class FlatDamageEffect : EffectStrategy
    {
        [LabelWidth(120)]
        [SerializeField] public int damage;
        [LabelWidth(120)]
        [SerializeField] public DamageType damageType;


        override public void StartEffect(GameObject user, IEnumerable<GameObject> targets)
        {
            int totalDamage = user.GetComponent<DefaultCharacter>().ProcessDamageDone(damage, damageType);
            foreach (var target in targets)
            {
                DefaultCharacter character = target.GetComponent<DefaultCharacter>();
                character.TakeDamage(totalDamage, damageType, user);
            }
        }
    }
}

