using Character.Character;
using Interaction;
using UnityEngine;

namespace UI
{
    public class PermanentCardsMenu : UICardMenu
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
            RemoveUICards();
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
            if (npc.CanRemoveCard())
            {
                player.RemovePermanentCard(GetCardSelected());
                RemoveUICards();
                InitializeCards();
                cardSelection.SetActive(false);
            }
        }
    }

}
