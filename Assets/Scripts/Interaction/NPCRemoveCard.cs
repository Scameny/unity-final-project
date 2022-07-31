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
            UIManager.manager.EnablePermanentCardsRemoveWindow(true);
            GameObject permanentWindow = UIManager.manager.GetPermanentCardsRemoveWindow();
            permanentWindow.GetComponentInChildren<PermanentCardsMenu>().SetNPCRemoveCard(this);
        }

        public override void OnEndInteract()
        {
            base.OnEndInteract();
            GameObject permanentWindow = UIManager.manager.GetPermanentCardsRemoveWindow();
            permanentWindow.GetComponentInChildren<PermanentCardsMenu>().SetNPCRemoveCard(null);
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