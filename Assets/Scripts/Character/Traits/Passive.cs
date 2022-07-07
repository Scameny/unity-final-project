using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

namespace Abilities.Passive
{
    [Serializable]
    [InlineProperty]
    public class Passive : IObserver<PassiveData>
    {
        [HideLabel]
        public PassiveAbility passiveAbility;
        protected IDisposable disposable;
        protected List<PassiveData> dataStored = new List<PassiveData>();

        #region Observer operations
        public void OnCompleted()
        {
            dataStored.Clear();
            disposable.Dispose();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(PassiveData value)
        {
            passiveAbility.Evaluate(dataStored, value);
        }

        public void SetDisposable(IDisposable disposable)
        {
            this.disposable = disposable;
        }
        #endregion
    }

}