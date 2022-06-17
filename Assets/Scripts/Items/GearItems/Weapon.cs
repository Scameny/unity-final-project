using Character.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "GearItem", menuName = "Items/Weapon", order = 2)]
    public class Weapon : GearItem
    {
        [SerializeField] AttackDamage attackDamage;


        public AttackDamage GetAttackDamage()
        {
            return attackDamage;
        }

        public override GearPiece GetSlotType()
        {
            return GearPiece.Weapon;
        }

        [System.Serializable]
        public class AttackDamage
        {
            public float minimAttack;
            public float maxAttack;
            public StatType scalingStat;
            public float scaleCoef;
            public DamageType damageType;
        }
    }
}