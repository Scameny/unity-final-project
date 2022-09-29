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
        private void LoadHero()
        {
            Instantiate(heroData.prefab);
        }

    }

}
