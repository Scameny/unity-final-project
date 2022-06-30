using Character.Character;
using Items;
using System.Collections.Generic;
using UnityEngine;

namespace Strategies.EffectStrategies
{
    [System.Serializable]
    public class WeaponScaleAttack : DamageEffectStrategy
    {
        public override int GetTotalDamage(GameObject user)
        {
            Weapon weapon = (Weapon)user.GetComponent<Hero>().GetItemBySlot(GearSlot.weapon);
            if (weapon != null)
            {
                int damage = Random.Range(weapon.GetAttackDamage().minimAttack, weapon.GetAttackDamage().maxAttack);
                damage += weapon.GetAttackDamage().scaleCoef * user.GetComponent<DefaultCharacter>().GetStatistic(weapon.GetAttackDamage().scalingStat);
                damage = user.GetComponent<DefaultCharacter>().ProcessDamageDone(damage, weapon.GetAttackDamage().damageType);
                return damage;
            }
            else
            {
                return 0;
            }
        }

        override protected void StartEffect(GameObject user, IEnumerable<GameObject> targets)
        {
            Weapon weapon = (Weapon)user.GetComponent<Hero>().GetItemBySlot(GearSlot.weapon);
            if (weapon == null)
                return;
            int damage = GetTotalDamage(user);
            foreach (var target in targets)
            {
                DefaultCharacter character = target.GetComponent<DefaultCharacter>();
                character.TakeDamage(damage, weapon.GetAttackDamage().damageType);
            }
        }
    }
}
