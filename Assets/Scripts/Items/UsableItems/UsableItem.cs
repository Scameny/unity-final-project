using Combat;
using Strategies.EffectStrategies;
using Strategies.FilterStrategies;
using Strategies.TargetingStrategies;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "UsableItem", menuName = "Items/Type of items/UsableItem", order = 1)]
    public class UsableItem : Item
    {
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

}
