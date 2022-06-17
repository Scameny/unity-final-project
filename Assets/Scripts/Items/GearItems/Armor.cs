using Sirenix.OdinInspector;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "GearItem", menuName = "Items/Armor", order = 2)]
    public class Armor : GearItem
    {
        [BoxGroup(GENERAL_SETTINGS_GROUP)]
        [SerializeField] GearPiece slot;

        public override GearPiece GetSlotType()
        {
            return slot;
        }
    }

}
