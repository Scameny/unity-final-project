using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FloorManagement
{
    [CreateAssetMenu(fileName = "RoomPool", menuName = "Map/Room pool")]
    public class RoomPool : ScriptableObject
    {

        public RoomInfo[] rooms;
        public RoomInfo bossRoom;
        public RoomInfo baseRoom;
        public EnemyPool enemyPool;

        public RoomInfo GetRoom()
        {
            int maxWeigth = 0;
            foreach (var room in rooms)
            {
                maxWeigth = room.weigth;
            }
            int index = 0;
            int lastIndex = rooms.Length - 1;
            while (index < lastIndex) {
                if (Random.Range(0, maxWeigth) < rooms[index].weigth)
                {
                    return rooms[index];
                }
                maxWeigth -= rooms[index].weigth;
                index++;
            }
            return rooms[lastIndex];
        }
    }

    [System.Serializable]
    public class RoomInfo
    {
        public GameObject roomGameObject;
        public int maxNumberOfRooms;
        public int weigth;
        public float heigth;
        public float width;
        public bool onlyOneWay;
        public bool needKey;
        public bool canSpawnEnemies;        
    }
}
