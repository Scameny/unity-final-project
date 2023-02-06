using Sirenix.OdinInspector;
using UnityEngine;

namespace CardSystem
{
    [System.Serializable]
    public class CardSwap
    {
        [SerializeField] Usable baseCard;
        [SerializeField] Usable improvedCard;


        public Usable GetBaseCard()
        {
            return baseCard;
        }

        public Usable GetImprovedCard()
        {
            return improvedCard;
        }
    }
}
