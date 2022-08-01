using Character.Character;
using UnityEngine;

namespace UI
{
    public class UICharacterCards : UICardMenu
    {
        [SerializeField] GameObject cardPrefab;
        Hero player;

        private void OnEnable()
        {
            if (player == null)
                player = GameObject.FindGameObjectWithTag("Player").GetComponent<Hero>();
            foreach (var usable in player.GetUsableCards())
            {
                GameObject card = Instantiate(cardPrefab, transform);
                card.GetComponent<UICard>().InitializeCard(usable);
            }
        }

        private void OnDisable()
        {
            RemoveUICards();
        }

       
    }

}
