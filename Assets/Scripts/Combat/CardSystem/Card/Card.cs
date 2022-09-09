using Combat;
using System.Collections.Generic;
using UI.Combat;
using UnityEngine;

namespace CardSystem
{
    public class Card : MonoBehaviour
    {

        bool oneUse;
        Usable cardEffect;
        GameObject user;
        UICombatCard uiCard;

        
        public void InitializeCard(Usable cardUse, GameObject user, bool oneUse)
        {
            uiCard = GetComponent<UICombatCard>();
            cardEffect = cardUse;
            uiCard.InitializeUICard(cardUse, oneUse);
            this.user = user;
            this.oneUse = oneUse;
        }

        public void SetVisibility(bool visible)
        {
            gameObject.SetActive(visible);
        }

        public void DestroyCard()
        {
            Destroy(gameObject);
        }

        public void UseCard()
        {
            cardEffect.Use(user, CombatManager.combatManager.GetCharactersInCombat(), this);
        }

        public void CancelCardUse()
        {
            uiCard.CancelCardUse();
        }


        public void CardEffectFinished()
        {
            user.GetComponent<TurnCombat>().CardUsed(this);
        }

        public IEnumerable<CardEffectType> GetCardEffect()
        {
            foreach (var item in cardEffect.GetCardEffectTypes())
            {
                yield return item;
            }
        }

        public List<ResourceCost> GetResourceCost()
        {
            return cardEffect.GetResourceCosts();
        }

        public bool IsOneUse()
        {
            return oneUse;
        }
        
        public Usable GetUsable()
        {
            return cardEffect;
        }

        public GameObject GetUser()
        {
            return user;
        }
    }
}
