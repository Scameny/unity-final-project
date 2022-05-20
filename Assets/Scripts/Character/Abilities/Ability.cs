using System.Collections.Generic;
using Combat;
using UnityEngine;
using NaughtyAttributes;
using Strategies.TargetingStrategies;
using Strategies.FilterStrategies;
using Strategies.EffectStrategies;

namespace Character.Abilities

{
    [CreateAssetMenu(fileName = "Ability", menuName = "Abilities/Ability", order = 1)]
    public class Ability : ScriptableObject
    {
        public bool hasSlots;
        [EnableIf("hasSlots")]
        public AbilitySlot slot;
        [SerializeField] TargetingStrategy targetingStrategy;
        [SerializeField] FilterStrategy[] filterStrategies;
        [SerializeField] EffectStrategy[] effectStrategies;
        

        public void Use(GameObject user, IEnumerable<GameObject> targets)
        {
            Debug.Log(user.name + ": Preparing " + name);
            user.GetComponent<TurnCombat>().TurnPreparationStop();
            foreach (var filterStrategy in filterStrategies)
            {
                targets = filterStrategy.Filter(targets);
            }
            targetingStrategy.AbilityTargeting(user, targets,
                (IEnumerable<GameObject> targets) =>
                {
                    TargetAquired(user, targets);
                });
        }

        private void TargetAquired(GameObject user, IEnumerable<GameObject> targets)
        {
            foreach (var effectStrategy in effectStrategies)
            {
                effectStrategy.StartEffect(user, targets);
            }
            Debug.Log(user.name + ": Used " + name);
            user.GetComponent<TurnCombat>().TurnPreparationResume();
        }
    }

    [System.Serializable]
    public class AbilitySlot
    {
        public int slotsNumber;
        public int slotRecharge;
        public SlotType slotType;
        public bool rechargeOnLevelUp;
    }

    public enum SlotType
    {
        Floor, Run, Room
    }


}
