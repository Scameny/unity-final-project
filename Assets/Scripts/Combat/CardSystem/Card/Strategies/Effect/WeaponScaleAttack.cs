using Character.Character;
using Items;
using System.Collections.Generic;
using UnityEngine;

namespace Strategies.EffectStrategies
{
    public class WeaponScaleAttack : EffectStrategy
    {
        public override void StartEffect(GameObject user, IEnumerable<GameObject> targets)
        {
            Weapon weapon = (Weapon) user.GetComponent<Hero>().GetItemBySlot(GearSlot.weapon);
            float damage = Random.Range(weapon.GetAttackDamage().minimAttack, weapon.GetAttackDamage().maxAttack);
            damage += weapon.GetAttackDamage().scaleCoef * user.GetComponent<DefaultCharacter>().GetStatistic(weapon.GetAttackDamage().scalingStat);
            damage = user.GetComponent<DefaultCharacter>().ProcessDamageDone(damage, weapon.GetAttackDamage().damageType);
            foreach (var target in targets)
            {
                DefaultCharacter character = target.GetComponent<DefaultCharacter>();
                character.TakeDamage(damage, weapon.GetAttackDamage().damageType);
            }
        }
    }
}
