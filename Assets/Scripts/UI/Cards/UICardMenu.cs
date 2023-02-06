using CardSystem;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Cards
{
    public class UICardMenu : MonoBehaviour
    {

        [TabGroup("Transform references")]
        [SerializeField] Transform cardSelection, viewport;

        Transform content;
        GameObject cardPrefab;


        private void Awake()
        {
            cardPrefab = Resources.Load<GameObject>("UI/UICard");
        }

        public void SetGrid()
        {
            content = GetComponentInChildren<ScrollRect>().content;
            GridLayoutGroup grid = GetComponentInChildren<GridLayoutGroup>();
            RectTransform cardRectTransform = cardPrefab.GetComponent<RectTransform>();
            RectTransform contentRectTransform = content.GetComponent<RectTransform>();
            float cardSizeRatio = cardRectTransform.rect.height / cardRectTransform.rect.width;
            float width = (contentRectTransform.rect.width - grid.padding.left - grid.padding.right - grid.spacing.x * grid.constraintCount) / grid.constraintCount;
            float heigth = cardSizeRatio * width;
            grid.cellSize = new Vector2(width, heigth);
            
        }

        public Transform GetContentTransform()
        {
            return content;
        }

        public Transform GetCardSelection()
        {
            if (cardSelection == null)
                content = GetComponentInChildren<ScrollRect>().content;
            return cardSelection;
        }

        public GameObject GetCardPrefab()
        {
            return cardPrefab;
        }

        public void OnSelection(Usable usable)
        {
            cardSelection.gameObject.SetActive(true);
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
            cardSelection.gameObject.SetActive(false);
        }
    }

}
