using CardSystem;
using UnityEngine;

namespace UI
{
    public class UICardMenu : MonoBehaviour
    {
        [SerializeField] protected Transform content;
        [SerializeField] protected GameObject cardSelection;
        [SerializeField] protected GameObject cardPrefab;

        public void OnSelection(Usable usable)
        {
            cardSelection.SetActive(true);
            cardSelection.GetComponentInChildren<UICard>().InitializeCard(usable, false);
        }

        protected Usable GetCardSelected()
        {
            return cardSelection.GetComponentInChildren<UICard>().GetCardUse();
        }

        protected void RemoveUICards()
        {
            for (int i = 0; i < content.childCount; i++)
            {
                Destroy(content.GetChild(i).gameObject);
            }
        }

        public void CancelSelection()
        {
            cardSelection.SetActive(false);
        }
    }

}
