using Character.Character;
using GameManagement.HeroGenerator;
using UnityEngine;

namespace GameManagemenet
{
    public class HeroSpawn : MonoBehaviour
    {
        [SerializeField] HeroSelectedData heroData;

        // Start is called before the first frame update
        void Awake()
        {
            LoadHero();
        }

        private void Start()
        {
            LoadItemsAndBuffs();
        }

        private void LoadHero()
        {
            Instantiate(heroData.prefab);
        }

        private void LoadItemsAndBuffs()
        {
            GameObject hero = GameObject.FindGameObjectWithTag("Player");
            hero.GetComponent<Hero>().AddItems(heroData.items);
            foreach (var trait in heroData.traits)
            {
                hero.GetComponent<Hero>().AddNewTrait(trait);
            }
        }

    }

}
