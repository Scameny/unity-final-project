using CardSystem;
using Combat;
using NaughtyAttributes;
using Strategies.EffectStrategies;
using Strategies.FilterStrategies;
using Strategies.TargetingStrategies;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "UsableItem", menuName = "Items/Type of items/UsableItem", order = 1)]
    public class UsableItem : Item, IUsable
    {
        ItemType type = ItemType.Consumable;

        [SerializeField] TargetingStrategy targetingStrategy;
        [SerializeField] FilterStrategy[] filterStrategies;
        [SerializeField] EffectStrategy[] effectStrategies;

        public CardType GetCardType()
        {
            return CardType.Item;
        }

        public override ItemType GetItemType()
        {
            return type;
        }

        public void Use(GameObject user, IEnumerable<GameObject> targets, Card card)
        {
            user.GetComponent<TurnCombat>().TurnPreparationPause();
            foreach (var filterStrategy in filterStrategies)
            {
                targets = filterStrategy.Filter(targets);
            }
            targetingStrategy.AbilityTargeting(user, targets,
                (IEnumerable<GameObject> targets, bool targetAquired) =>
                {
                    if (targetAquired)
                    {
                        TargetAquired(user, targets, card);
                        card.CardEffectFinished();
                    }
                    else
                        card.CancelCardUse();
                });
        }

        private void TargetAquired(GameObject user, IEnumerable<GameObject> targets, Card card)
        {
            foreach (var effectStrategy in effectStrategies)
            {
                effectStrategy.StartEffect(user, targets);
            }
            user.GetComponent<TurnCombat>().TurnPreparationResume();
        }


    }

}
