using Character.Character;
using GameManagement;
using Interaction;
using System;
using UnityEngine;

namespace UI
{
    public class UIPermanentCardsFrame : UICardMenu, IObserver<SignalData>
    {
        [SerializeField] GameObject cardPrefab;
        Hero player;
        NPCRemoveCard npc;

        private void OnEnable()
        {
            if (player == null)
                player = GameObject.FindGameObjectWithTag("Player").GetComponent<Hero>();
            InitializeCards();
        }

        public void SetNPCRemoveCard(NPCRemoveCard npc)
        {
            this.npc = npc;
        }

        private void OnDisable()
        {
            npc = null;
        }

        protected void InitializeCards()
        {
            foreach (var usable in player.GetPermanentCards())
            {
                GameObject card = Instantiate(cardPrefab, transform);
                card.GetComponent<UICard>().InitializeCard(usable);
            }
        }

        public void RemoveCard()
        {
            player.RemovePermanentCard(GetCardSelected());
            RemoveUICards();
            InitializeCards();
            npc.RemoveCard();
            cardSelection.SetActive(false);
            if (!npc.CanRemoveCard())
            {
                npc.OnEndInteract();
            }
        }

        public void CloseMenu()
        {
            RemoveUICards();
            npc.OnEndInteract();
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            Debug.LogError("Error on the remove card frame: " + error.Message);
        }

        public void OnNext(SignalData value)
        {
            if (value.signal.Equals(GameSignal.ASSIGN_NPC_UI_ELEMENT) && (value as UINpcSignalData).element.Equals(UIElement.REMOVE_CARD_FRAME))
            {
                this.npc = (value as UINpcSignalData).npc as NPCRemoveCard;
            }
        }
    }

}
