using Character.Character;
using GameManagement;
using Interaction;
using System;
using UI.Cards;
using UnityEngine;

namespace UI.Character.NPC
{
    public class UINPCRemovePermanentCardsFrame : UICardMenu, IObserver<SignalData>
    {
        Hero player;
        NPCRemoveCard npc;
        IDisposable disposable;

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Hero>();
            disposable = UIManager.manager.Subscribe(this);
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
                GameObject card = Instantiate(cardPrefab, content);
                card.GetComponent<UICard>().InitializeCard(usable);
            }
        }

        public void RemoveCard()
        {
            player.RemovePermanentCard(GetCardSelected());
            npc.RemoveCard();
            cardSelection.SetActive(false);
            RemoveUICards();
            if (!npc.CanRemoveCard())
            {
                npc.OnEndInteract();
            }
            InitializeCards();
        }

        public void CloseMenu()
        {
            RemoveUICards();
            npc.OnEndInteract();
        }

        public void OnCompleted()
        {
            disposable.Dispose();
        }

        public void OnError(Exception error)
        {
            Debug.LogError("Error on the remove card frame: " + error.Message);
        }

        public void OnNext(SignalData value)
        {
            if (value.signal.Equals(GameSignal.ASSIGN_NPC_UI_ELEMENT) && (value as UINpcSignalData).element.Equals(UIElement.REMOVE_CARD_FRAME))
            {
                npc = (value as UINpcSignalData).npc as NPCRemoveCard;
                InitializeCards();
                gameObject.SetActive((value as UINpcSignalData).enable);
            }
            else if(value.signal.Equals(GameSignal.ENABLE_UI_ELEMENT) && (value as UISignalData).element.Equals(UIElement.REMOVE_CARD_FRAME))
            {
                gameObject.SetActive((value as UISignalData).enable);
            } 
            else if (value.signal.Equals(GameSignal.START_GAME))
            {
                gameObject.SetActive(false);
            }
        }
    }

}
