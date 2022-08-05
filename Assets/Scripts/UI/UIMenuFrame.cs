using GameManagement;
using System;
using UnityEngine;

namespace UI
{
    public class UIMenuFrame : MonoBehaviour, IObserver<SignalData>
    {
        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(SignalData value)
        {
            if(value.signal.Equals(GameSignal.ENABLE_UI_ELEMENT) && (value as UISignalData).element.Equals(UIElement.MENU_FRAME))
            {
                gameObject.SetActive((value as UISignalData).enable);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            UIManager.manager.Subscribe(this);
        }
    }

}
