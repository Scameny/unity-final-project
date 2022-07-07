
using UI;
using UnityEngine;

namespace Interaction
{
    public class NPCRemoveCard : NPCInteractable
    {
        [SerializeField] int numRemoveCards;

        public override void Interact()
        {
            UIManager.manager.NPCInteraction(true);
            UIManager.manager.EnablePermanentCardsRemoveWindow(true);
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