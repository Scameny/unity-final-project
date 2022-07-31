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
            GameManager.gm.EnableSelectorInteraction(false);
            GameManager.gm.StartInteraction();
        }

        public abstract void Interact();

        public virtual void OnEndInteract()
        {
            UIManager.manager.NPCInteraction(false);
            UIManager.manager.EnablePermanentCardsRemoveWindow(false);
            GameManager.gm.EnableSelectorInteraction(true);
            GameManager.gm.EndInteraction();
        }

    }

}