using Sirenix.OdinInspector;
using UnityEngine;

namespace FloorManagement
{
    [CreateAssetMenu(fileName = "EnemyPool", menuName = "Map/Enemy pool")]
    public class EnemyPool : ScriptableObject
    {
        [SerializeField] EnemyInfo[] enemies;
        [SerializeField] EnemyInfo boss;

        public EnemyInfo GetRandomEnemy()
        {
            int maxWeigth = 0;
            foreach (var enemy in enemies)
            {
                maxWeigth += enemy.weigth;
            }
            int index = 0;
            int lastIndex = enemies.Length - 1;
            while (index < lastIndex)
            {
                if (Random.Range(0, maxWeigth) < enemies[index].weigth)
                {
                    return enemies[index];
                }
                maxWeigth -= enemies[index].weigth;
                index++;
            }
            return enemies[lastIndex];
        }
    }




    [System.Serializable]
    public class EnemyInfo
    {
        public GameObject gameObject;
        public bool hasMaxNumPerRoom;
        [EnableIf("hasMaxNumPerRoom")]
        public int maxNumPerRoom;
        public int weigth;
    }
}
