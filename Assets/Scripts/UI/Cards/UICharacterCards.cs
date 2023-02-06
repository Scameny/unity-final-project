using Character.Character;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UI.Cards
{
    public class UICharacterCards : UICardMenu
    {

        [TabGroup("Debug")]
        [SerializeField] int fakePopulateCards;


        Hero player;


        private void OnEnable()
        {
            Populate();
        }

        private void OnDisable()
        {
            RemoveUICards();
        }

        private void Populate()
        {
            if (player == null)
                player = GameObject.FindGameObjectWithTag("Player").GetComponent<Hero>();
            SetGrid();
            foreach (var usable in player.GetUsableCards())
            {
                GameObject card = Instantiate(GetCardPrefab(), GetContentTransform());
                card.GetComponent<UICard>().InitializeCard(usable);
            }
        }

        [Button]
        [TabGroup("Debug")]
        private void FakePopulate()
        {
            GameObject cardPrefab = Resources.Load<GameObject>("UI/UICard");
            for (int i = 0; i < fakePopulateCards; i++)
            {
                Instantiate(cardPrefab, GetContentTransform());
            }
        }

        [Button]
        [TabGroup("Debug")]
        private void RemoveCards()
        {
            while(GetContentTransform().childCount > 0)
            {
                DestroyImmediate(GetContentTransform().GetChild(0).gameObject);
            }
        }




    }

}
