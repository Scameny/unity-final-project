using CardSystem;
using Character.Character;
using Character.Stats;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace UI
{
    public class EnemyUI : CharacterUI
    {
        [SerializeField] float timeToFadeCardUsed;
        [SerializeField] GameObject cardPrefab;
        [SerializeField] Transform cardSlot;
        Slider resourceSlider;

        override protected void Awake()
        {
            base.Awake();
            resourceSlider = GetComponentInChildren<Slider>();
        }

        override protected void Update()
        {
            base.Update();
            UpdateResourceUnit();
        }

        private void UpdateResourceUnit()
        {
            resourceSlider.maxValue = GetCharacter().GetMaxValueOfResource(ResourceType.Health);
            resourceSlider.value = GetCharacter().GetCurrentResource(ResourceType.Health);
        }

        public void ShowCard(Card card)
        {
            GameObject cardUI = Instantiate(cardPrefab, cardSlot.transform);
            cardUI.GetComponent<UICard>().InitializeCard(card.GetUsable());
            cardUI.SetActive(true);
            cardUI.transform.position = cardSlot.transform.position;
            cardUI.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            cardUI.transform.DOMove(cardSlot.transform.position + Vector3.up, timeToFadeCardUsed).OnComplete(() =>
            {
                Destroy(cardUI);
            });
        }
    }
}