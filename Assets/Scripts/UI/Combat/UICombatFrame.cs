using GameManagement;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Combat
{
    public class UICombatFrame : MonoBehaviour, IObserver<SignalData>
    {
        GameObject player;
        DropZone dropZone;
        Button endTurnButton;
        IDisposable disposable;


        private void Start()
        {
            disposable = UIManager.manager.Subscribe(this);
            player = GameObject.FindGameObjectWithTag("Player");
            dropZone = GetComponentInChildren<DropZone>();
            endTurnButton = GetComponentInChildren<Button>();
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
            switch (value.signal)
            {
                case GameSignal.START_GAME:
                    gameObject.SetActive(false);
                    break;
                case GameSignal.START_COMBAT:
                    gameObject.SetActive(true);
                    endTurnButton.interactable = false;
                    break;

                case GameSignal.END_COMBAT:
                    if (gameObject.activeInHierarchy)
                        gameObject.SetActive(false);
                    break;
                case GameSignal.START_TURN:
                    if ((value as CombatSignalData).user.Equals(player))
                    {
                        endTurnButton.interactable = true;
                        dropZone.gameObject.SetActive(true);
                    }
                    break;
                case GameSignal.END_TURN:
                    if ((value as CombatSignalData).user.Equals(player))
                    {
                        endTurnButton.interactable = false;
                        dropZone.gameObject.SetActive(false);
                    }
                    break;
                case GameSignal.START_DRAGGING_CARD:
                    dropZone.GetComponent<Image>().raycastTarget = true;
                    break;
                case GameSignal.END_DRAGGING_CARD:
                    dropZone.GetComponent<Image>().raycastTarget = false;
                    break;
                case GameSignal.ENABLE_UI_ELEMENT:
                    if ((value as UISignalData).element.Equals(UIElement.COMBAT_FRAME))
                        gameObject.SetActive((value as UISignalData).enable);
                    break;
                default:
                    break;
            }
        }
    }

}
