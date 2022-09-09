using GameManagement;
using System;
using UnityEngine;

namespace UI.Frames
{
    public class UIMenuFrame : MonoBehaviour, IObserver<SignalData>
    {

        IDisposable disposable;
        public void OnCompleted()
        {
            disposable.Dispose();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(SignalData value)
        {
            if (value.signal.Equals(GameSignal.START_INTERACTION))
            {
                gameObject.SetActive(false);
            }
            else if (value.signal.Equals(GameSignal.END_INTERACTION))
            {
                gameObject.SetActive(true);
            }
            else if (value.signal.Equals(GameSignal.ENABLE_UI_ELEMENT) && (value as UISignalData).element.Equals(UIElement.MENU_FRAME))
            {
                gameObject.SetActive((value as UISignalData).enable);
            }
        }

        void Start()
        {
            disposable = UIManager.manager.Subscribe(this);
        }
    }

}
