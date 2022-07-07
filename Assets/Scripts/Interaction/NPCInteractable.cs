using DialogueEditor;
using UnityEngine;

namespace Interaction
{
    [RequireComponent(typeof(NPCConversation))]
    public abstract class NPCInteractable : MonoBehaviour
    {

        NPCConversation npcConversation;

        private void Awake()
        {
            npcConversation = GetComponent<NPCConversation>();
        }

        private void Start()
        {
            ConversationManager.Instance.StartConversation(npcConversation);
        }

        public NPCConversation GetConversation()
        {
            return npcConversation;
        }

        public abstract void Interact();

    }

}