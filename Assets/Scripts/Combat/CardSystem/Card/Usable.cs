using Character.Character;
using Character.Stats;
using Combat;
using Sirenix.OdinInspector;
using Strategies.EffectStrategies;
using Strategies.FilterStrategies;
using Strategies.TargetingStrategies;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CardSystem
{
    public abstract class Usable : SerializedScriptableObject
    {

        [HorizontalGroup("Base", Width = 150)]

        [HideLabel, PreviewField(140)]
        [SerializeField]
        [VerticalGroup("Base/Left")] 
        Sprite sprite;

        [SerializeField]
        [VerticalGroup("Base/Right")]
        string Name;


        [VerticalGroup("Base/Right")]
        [TextArea(4, 14)]
        [Space(4)]
        [SerializeField] string description;

        [VerticalGroup("Base/Right")]
        [SerializeField] UserAnimationType animationType;

        [VerticalGroup("Base/Right")]
        [SerializeField] AbilityType abilityType;

        [HorizontalGroup("Middle")]

        [BoxGroup("Middle/Strategies")]
        [TypeFilter("GetFilteredTargetingStrategyList")]
        [SerializeField] TargetingStrategy targetingStrategy;

        [HorizontalGroup("Middle/Strategies/Bottom")]

        [VerticalGroup("Middle/Strategies/Bottom/Left")]
        [HorizontalGroup("Middle/Strategies/Bottom/Left/Split")]
        [InlineProperty, HideLabel]
        [SerializeField] FilterStrategyList filterStrategiesList;
        
        
        [VerticalGroup("Middle/Strategies/Bottom/Right")]
        [HorizontalGroup("Middle/Strategies/Bottom/Right/Split")]
        [InlineProperty, HideLabel]
        [SerializeField] EffectStrategyList effectStrategiesList;


        [HorizontalGroup("Bottom")]
        [SerializeField] List<ResourceCost> resourceCosts = new List<ResourceCost>();

        #region Use operations
        public void Use(GameObject user, IEnumerable<GameObject> targets, Card card)
        {
            foreach (var cost in resourceCosts)
            {
                if (!user.GetComponent<DefaultCharacter>().HaveEnoughResource(cost.amount, cost.resourceType))
                    throw new NotEnoughResourceException(cost.resourceType);
            }

            foreach (var filterStrategy in filterStrategiesList.filterStrategies)
            {
                targets = filterStrategy.Filter(targets);
            }
            targetingStrategy.AbilityTargeting(user, targets,
                (IEnumerable<GameObject> targets, bool targetAquired) =>
                {
                    if (targetAquired)
                    {
                        TargetAquired(user, targets, card);
                    }
                    else
                    {
                        user.GetComponent<TurnCombat>().CancelCardUse(card);
                    }
                });

        }

        private void TargetAquired(GameObject user, IEnumerable<GameObject> targets, Card card)
        {
            user.GetComponent<TurnCombat>().StartCoroutine(EffectExecution(user, targets, card));
        }

        private IEnumerator EffectExecution(GameObject user, IEnumerable<GameObject> targets, Card card)
        {
            user.GetComponent<Animator>().Play(animationType.ToString());
            yield return new WaitForSeconds(user.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length);

            foreach (var resource in resourceCosts)
            {
                user.GetComponent<DefaultCharacter>().UseResource(resource.amount, resource.resourceType);
            }

            foreach (var effectStrategy in effectStrategiesList.effectStrategies)
            {
                effectStrategy.UseEffect(user, targets);
            }

            card.CardEffectFinished();
        }

        #endregion

        #region Resource operations
        public bool CanBeUsed(List<ResourceType> userResources)
        {
            foreach(var resource in resourceCosts)
            {
                if (!userResources.Contains(resource.resourceType))
                    return false;
            }
            return true;
        }
        #endregion

        #region Getters
        public Sprite GetSprite()
        {
            return sprite;
        }

        public string GetName()
        {
            return Name;
        }

        public string GetDescription()
        {
            return description;
        }

        public AbilityType GetAbilityType()
        {
            return abilityType;
        }

        public List<ResourceCost> GetResourceCosts()
        {
            return resourceCosts;
        }

        public IEnumerable<CardEffectType> GetCardEffectTypes()
        {
            foreach (var item in effectStrategiesList.effectStrategies)
            {
                foreach (var type in item.GetCardEffectType())
                {
                    yield return type;
                }
            }
        }
        abstract public CardType GetCardType();
        #endregion

        #region simulation operations

        public int GetSimulatedDamage(GameObject user) 
        {
            int totalDamage = 0;
            foreach (var item in effectStrategiesList.effectStrategies)
            {
                if (item.GetCardEffectType().Contains(CardEffectType.Damage))
                    totalDamage += (item as DamageEffectStrategy).GetTotalDamage(user);
            }
            return totalDamage;
        }

        public int GetSimulatedResourceGained(GameObject user, ResourceType resourceType)
        {
            int resourceGained = 0;
            foreach (var item in effectStrategiesList.effectStrategies)
            {
                if (item.GetCardEffectType().Contains(CardEffectType.ResourceGain) && (item as ResourceGainEffectStrategy).GetResourceType().Equals(resourceType))
                    resourceGained += (item as ResourceGainEffectStrategy).GetResourceAmountGained();
            }
            return resourceGained;
        }

        public IEnumerable<ResourceType> GetResourceGained()
        {
            foreach (var item in effectStrategiesList.effectStrategies)
            {
                yield return (item as ResourceGainEffectStrategy).GetResourceType();
            }
        }

        #endregion

        #region Object operations
        public override bool Equals(object obj)
        {
            return obj is Usable usable &&
                   base.Equals(obj) &&
                   Name == usable.Name;
        }

        public override int GetHashCode()
        {
            int hashCode = 890389916;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            return hashCode;
        }
        #endregion

        #region Editor utils
        public IEnumerable<Type> GetFilteredTargetingStrategyList()
        {
            var q = typeof(TargetingStrategy).Assembly.GetTypes()
                .Where(x => !x.IsAbstract)                                          // Excludes BaseClass
                .Where(x => typeof(TargetingStrategy).IsAssignableFrom(x));                 // Excludes classes not inheriting from BaseClass
            return q;
        }

        #endregion
    }


    [Serializable]
    public class UsableCard
    {
        [HideLabel]
        [HorizontalGroup("Usable")]
        [ValidateInput("ValidateUsableNotNull", "Usable can't be null")]
        public Usable usable;
        [HideLabel]
        [HorizontalGroup("Usable")]
        [ValidateInput("ValidateQuantityMoreThanZero", "Quantity must have a value greater than zero")]
        [Space(16)]
        public int quantity;

        private bool ValidateUsableNotNull(Usable usable)
        {
            return usable != null;
        }

        private bool ValidateQuantityMoreThanZero(int quantity)
        {
            return quantity > 0;
        }

    }

    public enum CardType
    {
        Ability,
        Item
    }

    public struct ResourceCost
    {
        public ResourceType resourceType;
        public int amount;
    }

    public enum CardEffectType
    {
        Damage,
        Buff,
        Debuff,
        Heal,
        ResourceGain,
        DrawCards
    }

    public enum UserAnimationType
    {
        PhysicAttack,
        Cast,
        Hurt
    }

    public enum AbilityType
    {
        Physic,
        Fire,
        Ice,
        Electic,
        Arcane,
        Nature,
        None
    }
}
