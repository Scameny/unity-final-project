using Animations.Ability;
using GameManagement;
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
        protected abstract List<SignalData> EffectAction(CombatSignalData passiveData);

        [HideLabel, InlineProperty]
        [SerializeReference] IPassiveSpellAnimation spellAnimation;

        public void EffectActivation(SignalData passiveData)
        {
            List<SignalData> signalDatas = EffectAction(passiveData as CombatSignalData);
            if (signalDatas.Count == 0)
                return;
            if (spellAnimation != null)
                spellAnimation.PlaySpellAnimation(passiveData as CombatSignalData, signalDatas);
            else
                UI.UIManager.manager.SendData(signalDatas);

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
