using DialogueEditor;
using GameManagement;
using UI;
using UnityEngine;

namespace Interaction
{
    public class NPCRemoveCard : NPCInteractable
    {
        [SerializeField] int numRemoveCards;
        [SerializeField] NPCConversation removeCardConversation;
        [SerializeField] NPCConversation defaultConversation;

        public override void StartConversation()
        {
            base.StartConversation();
            if (numRemoveCards > 0)
                ConversationManager.Instance.StartConversation(removeCardConversation);
            else
                ConversationManager.Instance.StartConversation(defaultConversation);
        }

        public override void Interact()
        {
            UIManager.manager.SendData(new UINpcSignalData(GameSignal.ASSIGN_NPC_UI_ELEMENT, UIElement.REMOVE_CARD_FRAME, true, this));
        }

        public override void OnEndInteract()
        {
            base.OnEndInteract();
            UIManager.manager.SendData(new UISignalData(GameSignal.ENABLE_UI_ELEMENT, UIElement.REMOVE_CARD_FRAME, false));
        }

        public void RemoveCard()
        {
            numRemoveCards -= 1;
        }

        public bool CanRemoveCard()
        {
            return numRemoveCards > 0;
        }

        
    }
}