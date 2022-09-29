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
        [ValueDropdown("CustomAddClassesButton", IsUniqueList = true, DrawDropdownForListElements = false, DropdownTitle = "StartingItems")]
        [ListDrawerSettings(DraggableItems = false, Expanded = true)]
        [SerializeField] List<HeroInitialVariables> heroVariables = new List<HeroInitialVariables>();
        [SerializeField] List<GameObject> heroFrames;

        List<Item> items;
        List<BaseBuff> traits;

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
            List<PlayerClass> allClasses = new List<PlayerClass>(Resources.FindObjectsOfTypeAll<PlayerClass>());
            List<PlayerClass> heroClasses = new List<PlayerClass>();
            for (int i = 0; i < heroFrames.Count; i++)
            {
                heroClasses.Add(allClasses[UnityEngine.Random.Range(0, allClasses.Count)]);
            }

            int count = 0;
            foreach (var heroClass in heroClasses)
            {
                HeroInitialVariables heroInitialVariables = heroVariables.Find(h => h.GetHeroClass().Equals(heroClass));
                int numOfItems = UnityEngine.Random.Range(0, heroInitialVariables.GetMaxNumOfItems() + 1);
                if (numOfItems > 0)
                {
                    items = UtilsClass.instance.GetListFromPool(heroInitialVariables.GetItems(), numOfItems, true);
                } 
                else
                {
                    items = new List<Item>();
                }
                int numOfTraits = UnityEngine.Random.Range(0, heroInitialVariables.GetMaxNumOfTraits() + 1);
                if (numOfTraits > 0)
                {
                    traits = UtilsClass.instance.GetListFromPool(heroInitialVariables.GetBaseTraits(), numOfTraits, true);
                }
                else
                {
                    traits = new List<BaseBuff>();
                }
                heroFrames[count].GetComponent<UIHeroSelectionFrame>().SetVariables(items, traits, heroClass, heroInitialVariables.GetSprite(), heroInitialVariables.GetPrefab());
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
                .Except(heroVariables.Select(x => x.GetHeroClass()))
                .Select(x => new HeroInitialVariables(x))
                .AppendWith(this.heroVariables)
                .Select(x => new ValueDropdownItem(x.GetHeroClass().ToString(), x));
        }


        [Serializable]
        class HeroInitialVariables
        {
            public HeroInitialVariables(PlayerClass heroClass)
            {
                this.heroClass = heroClass;
            }

            [SerializeField] PlayerClass heroClass;
            [SerializeField] int maxNumOfItems;
            [SerializeField] Sprite sprite;
            [SerializeField] GameObject prefab;
            [SerializeField] List<PoolObject<Item>> items = new List<PoolObject<Item>>();
            [SerializeField] int maxNumOfTraits;
            [SerializeField] List<PoolObject<BaseBuff>> baseTraits = new List<PoolObject<BaseBuff>>();

            public PlayerClass GetHeroClass()
            {
                return heroClass;
            }
            public List<PoolObject<Item>> GetItems()
            {
                return items;
            }
            
            public List<PoolObject<BaseBuff>> GetBaseTraits()
            {
                return baseTraits;
            }

            public Sprite GetSprite()
            {
                return sprite;
            }

            public int GetMaxNumOfItems()
            {
                return maxNumOfItems;
            }

            public int GetMaxNumOfTraits()
            {
                return maxNumOfTraits;
            }

            public GameObject GetPrefab()
            {
                return prefab;
            }

        }
    }


   
}