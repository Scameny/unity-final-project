using Animations;
using CardSystem;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Strategies.EffectStrategies

{
    [Serializable]
    public abstract class EffectStrategy
    {
        protected List<CardEffectType> effectTypes = new List<CardEffectType>();
        [HideLabel, InlineProperty]
        [SerializeReference] ISpellAnimation spellAnimation;

        abstract protected void StartEffect(GameObject user, IEnumerable<GameObject> targets);


        public void UseEffect(GameObject user, IEnumerable<GameObject> targets)
        {
            if (spellAnimation != null)
                spellAnimation.PlaySpellAnimation(user, targets, StartEffect);
            else
                StartEffect(user, targets);
        }

        public List<CardEffectType> GetCardEffectType()
        {
            return effectTypes;
        }

        public IEnumerable<Type> GetFilteredAnimations()
        {
            var q = typeof(ISpellAnimation).Assembly.GetTypes()
                .Where(x => !x.IsAbstract)                                          // Excludes BaseClass
                .Where(x => typeof(ISpellAnimation).IsAssignableFrom(x));                 // Excludes classes not inheriting from BaseClass
            return q;
        }
    }
}
