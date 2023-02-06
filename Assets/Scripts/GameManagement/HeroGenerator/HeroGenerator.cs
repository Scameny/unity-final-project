using Character.Buff;
using Character.Classes;
using Items;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UI.GameMenu;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace GameManagement.HeroGenerator
{
    public class HeroGenerator : MonoBehaviour
    {
        [ValueDropdown("CustomAddClassesButton", DrawDropdownForListElements = false, DropdownTitle = "Classes")]
        [ListDrawerSettings(DraggableItems = false, Expanded = true)]
        [SerializeField] List<PoolObject<HeroDataForGenerator>> possibleHeroes = new List<PoolObject<HeroDataForGenerator>>();
        [SerializeField] List<GameObject> heroFrames;


        public static HeroGenerator Instance;

        HeroSelectedData heroData;

        private void Awake()
        {
            Instance = this;
            heroData = Resources.Load<HeroSelectedData>("GameData/HeroData");
        }

        private void Start()
        {
            GenerateHeroes();
        }
       

        [Button]
        private void GenerateHeroes()
        {
            List<HeroDataForGenerator> heroes = UtilsClass.instance.GetListFromPool(possibleHeroes, 3, true);

            int count = 0;
            foreach (var hero in heroes)
            {
                heroFrames[count].GetComponent<UIHeroSelectionFrame>().SetVariables(hero.GetItems(), hero.GetBaseTraits(), hero.GetHeroClass(), hero.GetSprite(), hero.GetPrefab());
                count++;
            }
        }

        public void StartGame(CharacterClass characterClass, List<Item> items, List<BaseBuff> traits, GameObject prefab)
        {
            StartCoroutine(StartGameCoroutine(characterClass, items, traits, prefab));
        }


        private IEnumerator StartGameCoroutine(CharacterClass characterClass, List<Item> items, List<BaseBuff> traits, GameObject prefab)
        {
            heroData.heroClass = characterClass;
            heroData.items = items;
            heroData.traits = traits;
            heroData.prefab = prefab;
            AsyncOperation sceneAsyncOp = SceneManager.LoadSceneAsync("GameScene");
            while (!sceneAsyncOp.isDone)
            {
                yield return null;
            }
        }

        private IEnumerable CustomAddClassesButton()
        {
            List<PlayerClass> allClasses = new List<PlayerClass>(Resources.FindObjectsOfTypeAll<PlayerClass>());
            return allClasses
                .Select(x => new PoolObject<HeroDataForGenerator>(0, new HeroDataForGenerator(x)))
                .AppendWith(this.possibleHeroes)
                .Select(x => new ValueDropdownItem(x.gameObject.GetHeroClass().ToString(), x));
        }


        [Serializable]
        class HeroDataForGenerator
        {
            public HeroDataForGenerator(PlayerClass heroClass)
            {
                this.heroClass = heroClass;
            }

            [SerializeField] PlayerClass heroClass;
            [SerializeField] Sprite sprite;
            [SerializeField] GameObject prefab;
            [SerializeField] List<Item> items = new List<Item>();
            [SerializeField] List<BaseBuff> baseTraits = new List<BaseBuff>();

            public PlayerClass GetHeroClass()
            {
                return heroClass;
            }
            public List<Item> GetItems()
            {
                return items;
            }
            
            public List<BaseBuff> GetBaseTraits()
            {
                return baseTraits;
            }

            public Sprite GetSprite()
            {
                return sprite;
            }

            public GameObject GetPrefab()
            {
                return prefab;
            }

        }
    }


   
}