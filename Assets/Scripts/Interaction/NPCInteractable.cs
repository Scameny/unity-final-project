using GameManagement;
using UI;
using UnityEngine;

namespace Interaction
{
    public abstract class NPCInteractable : MonoBehaviour
    {

        virtual public void StartConversation() 
        {
            UIManager.manager.NPCInteraction(true);
            GameManager.gm.EnableSelectorInCurrentRoom(false);
            GameManager.gm.StartInteraction();
        }

        public abstract void Interact();

        public virtual void OnEndInteract()
        {
            UIManager.manager.NPCInteraction(false);
            UIManager.manager.SendData(new UISignalData(GameSignal.ENABLE_UI_ELEMENT, UIElement.REMOVE_CARD_FRAME, true));
            GameManager.gm.EnableSelectorInCurrentRoom(true);
            GameManager.gm.EndInteraction();
        }

    }

}