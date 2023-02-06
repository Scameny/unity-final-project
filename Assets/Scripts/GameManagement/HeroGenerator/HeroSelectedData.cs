using Character.Buff;
using Character.Classes;
using Items;
using System.Collections.Generic;
using UnityEngine;


namespace GameManagement.HeroGenerator
{
    [CreateAssetMenu(fileName = "HeroData", menuName = "GameData/HeroSelectedData")]
    public class HeroSelectedData : ScriptableObject
    {
        public GameObject prefab;
        public CharacterClass heroClass;
        public List<Item> items;
        public List<BaseBuff> traits;
    }

}
