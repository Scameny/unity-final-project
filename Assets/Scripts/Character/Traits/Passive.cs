using Sirenix.OdinInspector;
using System;

namespace Abilities.Passive
{
    [Serializable]
    [InlineProperty]
    public class Passive : IObserver<PassiveData>
    {
        [HideLabel]
        public PassiveAbility passiveAbility;
        protected IDisposable disposable;

        public Passive(PassiveAbility ability)
        {
            passiveAbility = ability;
        }

        #region Observer operations
        public void OnCompleted()
        {
            disposable.Dispose();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(PassiveData value)
        {
            foreach (var effect in passiveAbility.GetPassiveEffectStrategyList())
            {
                effect.Evaluate(value.signalType, value.user, value.targets);
            }
        }

        public void SetDisposable(IDisposable disposable)
        {
            this.disposable = disposable;
        }
        #endregion
    }

}