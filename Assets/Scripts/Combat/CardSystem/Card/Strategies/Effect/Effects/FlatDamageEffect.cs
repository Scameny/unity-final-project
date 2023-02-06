using System.Collections.Generic;
using UnityEngine;
using Character.Character;
using Sirenix.OdinInspector;
using GameManagement;

namespace Strategies.EffectStrategies
{
    [System.Serializable]
    public class FlatDamageEffect : DamageEffectStrategy
    {
        [LabelWidth(120)]
        [SerializeField] int damage;
        [LabelWidth(120)]
        [SerializeField] DamageType damageType;

        public override int GetTotalDamage(GameObject user)
        {
            return user.GetComponent<DefaultCharacter>().ProcessDamageDone(damage, damageType);
        }

        override protected List<SignalData> StartEffect(GameObject user, IEnumerable<GameObject> targets)
        {
            List<SignalData> signalDatas = new List<SignalData>();
            int totalDamage = GetTotalDamage(user);
            foreach (var target in targets)
            {
                DefaultCharacter character = target.GetComponent<DefaultCharacter>();
                signalDatas.AddRange(character.TakeDamage(totalDamage, damageType));
            }
            return signalDatas;
        }
    }
}

