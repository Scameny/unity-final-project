using Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Character.Reward { 
    public class Reward : ScriptableObject
    {
        public float exp;

    }

    [System.Serializable]
    public class LootItemInfo
    {
        public Item item;
        public float prob;
    }
}
