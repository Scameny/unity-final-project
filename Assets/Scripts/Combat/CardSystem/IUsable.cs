using CardSystem;
using Strategies.FilterStrategies;
using Strategies.TargetingStrategies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardSystem
{
    public interface IUsable
    {
        public void Use(GameObject user, IEnumerable<GameObject> targets, Card card);

        public Sprite GetSprite();

        public CardType GetCardType();
    }

    public enum CardType
    {
        BasicAttack,
        Ability,
        Item
    }
}
