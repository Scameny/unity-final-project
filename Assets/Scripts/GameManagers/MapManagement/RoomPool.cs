using Sirenix.OdinInspector;
using UnityEngine;

namespace FloorManagement
{
    [CreateAssetMenu(fileName = "RoomPool", menuName = "Map/Room pool")]
    public class RoomPool : ScriptableObject
    {
        [SerializeField] RoomInfo[] rooms;
        [SerializeField] RoomInfo bossRoom = new RoomInfo(true);
        [SerializeField] RoomInfo baseRoom = new RoomInfo(true);
        [SerializeField] EnemyPool enemyPool;

        public RoomInfo[] GetRooms()
        {
            return rooms;
        }

        public RoomInfo GetBossRoom()
        {
            return bossRoom;
        }

        public RoomInfo GetBaseRoom()
        {
            return baseRoom;
        }

        public EnemyPool GetEnemyPool()
        {
            return enemyPool;
        }
    }
   
    [System.Serializable]
    public class RoomInfo
    {
        public RoomInfo(bool uniqueRoom)
        {
            this.uniqueRoom = uniqueRoom;
        }

        public RoomInfo()
        {
            this.uniqueRoom = false;
        }

        public GameObject roomGameObject;
        bool uniqueRoom;
        [HideIf("uniqueRoom")]
        public int maxNumberOfRooms;
        [HideIf("uniqueRoom")]
        public int weigth;
        public float heigth;
        public float width;
        [HideIf("uniqueRoom")]
        public bool onlyOneWay;
        [HideIf("uniqueRoom")]
        public bool needKey;
    }
}
