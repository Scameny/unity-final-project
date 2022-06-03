using CardSystem;
using Combat;
using NaughtyAttributes;
using Strategies.EffectStrategies;
using Strategies.FilterStrategies;
using Strategies.TargetingStrategies;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace Abilities.BasicAttack
{
    [CreateAssetMenu(fileName = "BasicAttack", menuName = "Abilities/BasicAttack", order = 1)]
    public class BasicAttack : ScriptableObject, IUsable
    {

        [ShowAssetPreview]
        public Sprite sprite;
        [SerializeField] TargetingStrategy targetingStrategy;
        [SerializeField] FilterStrategy[] filterStrategies;
        [SerializeField] EffectStrategy[] effectStrategies;

        public CardType GetCardType()
        {
            return CardType.BasicAttack;
        }

        public Sprite GetSprite()
        {
            return sprite;
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
                        TargetAquired(user, targets);
                        card.CardEffectFinished();
                    }
                    else
                        card.CancelCardUse();
                });

        }

        private void TargetAquired(GameObject user, IEnumerable<GameObject> targets)
        {
            foreach (var effectStrategy in effectStrategies)
            {
                effectStrategy.StartEffect(user, targets);
            }
            user.GetComponent<TurnCombat>().TurnPreparationResume();
        }

    }

    [System.Serializable]
    public class BasicAttackCard
    {
        public BasicAttack usable;
        public int quantity;
    }
}
