using GameManagement;
using System;
using UnityEngine;

namespace UI.UIElements
{
    public class UICharacterMenu : MonoBehaviour, IObserver<SignalData>
    {

        IDisposable disposable;

        private void Start()
        {
            disposable = UIManager.manager.Subscribe(this);    
        }

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
            if (value.signal.Equals(GameSignal.OPEN_CHARACTER_MENU))
            {
                gameObject.SetActive(true);
                GameManager.gm.OpenMenu();
            } 
            else if (value.signal.Equals(GameSignal.CLOSE_CHARACTER_MENU))
            {
                gameObject.SetActive(false);
                GameManager.gm.CloseMenu();
            } else if (value.signal.Equals(GameSignal.START_GAME))
            {
                gameObject.SetActive(false);
            }

        }
    }

}
