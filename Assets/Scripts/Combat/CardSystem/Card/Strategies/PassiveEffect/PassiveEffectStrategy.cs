using Abilities.Passive;
using Animations;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Strategies.PassiveEffectStrategies
{
    [Serializable]
    public abstract class PassiveEffectStrategy
    {
        protected abstract void EffectAction(PassiveData passiveData);

        [HideLabel, InlineProperty]
        [SerializeReference] IPassiveSpellAnimation spellAnimation;

        public void EffectActivation(PassiveData passiveData)
        {
            if (spellAnimation != null)
                spellAnimation.PlaySpellAnimation(passiveData, EffectAction);
            else
                EffectAction(passiveData);
        }

        public IEnumerable<Type> GetFilteredAnimations()
        {
            var q = typeof(IPassiveSpellAnimation).Assembly.GetTypes()
                .Where(x => !x.IsAbstract)                                          // Excludes BaseClass
                .Where(x => typeof(IPassiveSpellAnimation).IsAssignableFrom(x));                 // Excludes classes not inheriting from BaseClass
            return q;
        }
    }

}
