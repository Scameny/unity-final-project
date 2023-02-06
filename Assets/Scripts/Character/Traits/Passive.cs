using GameManagement;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

namespace Abilities.Passive
{
    [Serializable]
    [InlineProperty]
    public class Passive : IObserver<SignalData>
    {
        [HideLabel]
        public PassiveAbility passiveAbility;
        protected IDisposable disposable;
        protected List<SignalData> dataStored = new List<SignalData>();

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

        public void OnNext(SignalData value)
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