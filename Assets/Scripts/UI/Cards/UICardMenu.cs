using CardSystem;
using UnityEngine;

namespace UI
{
    public class UICardMenu : MonoBehaviour
    {
        [SerializeField] protected GameObject cardSelection;

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
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }

        public void CancelSelection()
        {
            cardSelection.SetActive(false);
        }
    }

}
