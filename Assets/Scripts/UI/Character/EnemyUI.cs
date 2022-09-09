using CardSystem;
using Character.Stats;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using GameManagement;
using UI.Cards;

namespace UI.Character
{

    /// <summary>
    /// Enemy UI script. Show cards when are played and update health values
    /// </summary>
    public class EnemyUI : CombatCharacterUI
    {
        [SerializeField] float timeToFadeCardUsed;
        [SerializeField] GameObject cardPrefab;
        [SerializeField] Transform cardSlot;
        Slider resourceSlider;

        override protected void Start()
        {
            base.Start();
            resourceSlider = GetComponentInChildren<Slider>();
        }
      

        private void ShowCard(Card card)
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

        override public void OnNext(SignalData signalData)
        {
            base.OnNext(signalData);
            if (signalData.signal.Equals(GameSignal.CARD_PLAYED) && (signalData as CombatCardSignalData).user.Equals(GetCharacter().gameObject))
            {
                ShowCard((signalData as CombatCardSignalData).card);
            }
            else if (signalData.signal.Equals(GameSignal.RESOURCE_MODIFY) && (signalData as CombatResourceSignalData).user.Equals(GetCharacter().gameObject))
            {
                resourceSlider.maxValue = GetCharacter().GetMaxValueOfResource(ResourceType.Health);
            } 
            else if (signalData.signal.Equals(GameSignal.MAX_RESOURCE_MODIFY) && (signalData as CombatResourceSignalData).user.Equals(GetCharacter().gameObject))
            {
                resourceSlider.maxValue = GetCharacter().GetMaxValueOfResource(ResourceType.Health);
            }

        }
    }
}