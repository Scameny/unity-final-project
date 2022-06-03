using Character.Character;
using Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Strategies.EffectStrategies
{
    [CreateAssetMenu(fileName = "BasicAttackEffect", menuName = "Strategy/EffectStrategy/BasicAttackEffect", order = 1)]
    public class WeaponScaleAttack : EffectStrategy
    {
        public override void StartEffect(GameObject user, IEnumerable<GameObject> targets)
        {
            GearItem weapon = user.GetComponent<DefaultCharacter>().GetItemBySlot(GearSlot.weapon);
            float damage = Random.Range(weapon.attackDamage.minimAttack, weapon.attackDamage.maxAttack);
            damage += weapon.attackDamage.scaleCoef * user.GetComponent<DefaultCharacter>().GetStatistic(weapon.attackDamage.scalingStat);
            damage = user.GetComponent<DefaultCharacter>().ProcessDamageDone(damage, weapon.attackDamage.damageType);
            foreach (var target in targets)
            {
                DefaultCharacter character = target.GetComponent<DefaultCharacter>();
                character.TakeDamage(damage, weapon.attackDamage.damageType);
            }
        }
    }
}
