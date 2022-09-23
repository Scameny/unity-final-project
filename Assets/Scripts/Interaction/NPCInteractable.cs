using GameManagement;
using UnityEngine;

namespace Interaction
{
    public abstract class NPCInteractable : MonoBehaviour
    {

        virtual public void StartConversation() 
        {
            GameManager.gm.EnableSelectorInCurrentRoom(false);
            GameManager.gm.StartInteraction();
        }

        public abstract void Interact();

        public virtual void OnEndInteract()
        {
            GameManager.gm.EnableSelectorInCurrentRoom(true);
            GameManager.gm.EndInteraction();
        }

    }

}