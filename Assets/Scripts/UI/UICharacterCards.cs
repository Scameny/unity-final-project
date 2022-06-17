using Character.Character;
using Combat;
using UnityEngine;

namespace UI
{
    public class UICharacterCards : MonoBehaviour
    {
        [SerializeField] GameObject cardPrefab;
        Hero player;

        private void OnEnable()
        {
            if (player == null)
                player = GameObject.FindGameObjectWithTag("Player").GetComponent<Hero>();
            foreach (var usable in player.GetUsableCards())
            {
                Debug.Log(usable.name);
                GameObject card = Instantiate(cardPrefab, transform);
                SimpleTooltip tooltip = card.GetComponent<SimpleTooltip>();
                tooltip.infoLeft = usable.name + "\n" + usable.GetDescription();
            }
        }

        private void OnDisable()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
    }

}
