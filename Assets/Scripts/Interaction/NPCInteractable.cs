using GameManagement;
using UI;
using UnityEngine;

namespace Interaction
{
    public abstract class NPCInteractable : MonoBehaviour
    {

        virtual public void StartConversation() 
        {
            GameManager.gm.EnableSelectorInCurrentRoom(false);
            GameManager.gm.StartInteraction();
            UIManager.manager.SendData(new SignalData(GameSignal.START_INTERACTION));
        }

        public abstract void Interact();

        public virtual void OnEndInteract()
        {
            GameManager.gm.EnableSelectorInCurrentRoom(true);
            GameManager.gm.EndInteraction();
            UIManager.manager.SendData(new SignalData(GameSignal.END_INTERACTION));
        }

    }

}