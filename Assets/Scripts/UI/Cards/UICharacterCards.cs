using Character.Character;
using UnityEngine;

namespace UI.Cards
{
    public class UICharacterCards : UICardMenu
    {
        Hero player;

        private void OnEnable()
        {
            if (player == null)
                player = GameObject.FindGameObjectWithTag("Player").GetComponent<Hero>();
            foreach (var usable in player.GetUsableCards())
            {
                GameObject card = Instantiate(cardPrefab, content);
                card.GetComponent<UICard>().InitializeCard(usable);
            }
        }

        private void OnDisable()
        {
            RemoveUICards();
        }

    }

}
