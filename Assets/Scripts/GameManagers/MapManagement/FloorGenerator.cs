using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace FloorManagement { 
    
    public class FloorGenerator : MonoBehaviour
    {
        [HideInInspector]
        public List<Room> rooms { get; private set; } = new List<Room>();

        [Header("Weigths for generation")]
        public float pathContinue = 65.0f;
        public float[] pathContinueRegression = new float[4] { 0.0f, 5.0f, 5.0f, 10.0f }; 

        private RoomPool roomPool;
        private int maxNumOfRooms;

        private int maxX;
        private int maxY;
        private bool bossRoom = false;


        public void GenerateFloor(int maxNumOfRooms, RoomPool roomPool)
        {
            this.maxNumOfRooms = maxNumOfRooms;
            this.roomPool = roomPool;
            BuildBaseFloor(roomPool.baseRoom);
            foreach (var room in rooms)
            {
                EnableDoors(room);
            }
            OnCreated();
        }

        public RoomManager GetBaseRoom()
        {
            return rooms.First(r => r.i == 0 && r.j == 0).gameObject.GetComponent<RoomManager>();
        }

        public RoomManager GetRoom(RoomManager room, Direction dir)
        {
            Room currentRoom = rooms.First(r => r.gameObject.GetInstanceID().Equals(room.gameObject.GetInstanceID()));
            int x = 0;
            int y = 0;
            UtilsClass.instance.GetDirection(dir, ref x, ref y);
            return rooms.First(r => r.i == currentRoom.i + x && r.j == currentRoom.j + y).gameObject.GetComponent<RoomManager>();
        }

        public void BuildBaseFloor(RoomInfo baseRoom)
        {
            Room room = new Room(baseRoom, 0, 0);
            room.gameObject = Instantiate(baseRoom.roomGameObject, new Vector3(0, 0, 0), Quaternion.identity);
            rooms.Add(room);
            maxX = 0;
            maxY = 0;
            BuildNewRoom(room, 0);
        }


        private void BuildNewRoom(Room room, int pathContInd)
        {
            int x = 0;
            int y = 0;
            GetRandomDirection(ref x, ref y);
            while (rooms.Any(r => r.i == room.i + x && r.j == room.j + y))
            {
                GetRandomDirection(ref x, ref y);
            }
            RoomInfo roomToInstantiate = null;
            if (bossRoom)
            {
                roomToInstantiate = roomPool.bossRoom;
            }
            else
            {
                roomToInstantiate = GetNewRoom();
            }
            Room newRoom = new Room(roomToInstantiate, room.i + x, room.j + y);
            newRoom.gameObject = Instantiate(roomToInstantiate.roomGameObject, room.gameObject.transform.position + new Vector3(room.roomInfo.width * x, room.roomInfo.heigth * y, 0), Quaternion.identity);
            rooms.Add(newRoom);

            if (rooms.Count < maxNumOfRooms)
            {    
                if (Mathf.Abs(maxX) + Mathf.Abs(maxY) < Mathf.Abs(newRoom.i) + Mathf.Abs(newRoom.j))
                {
                    maxX = newRoom.i;
                    maxY = newRoom.j;
                }
                

                if (room.roomInfo.onlyOneWay)
                {
                    BuildNewRoom(room, pathContInd);
                }
                else if (Random.Range(0.0f, 100.0f) < pathContinue - pathContinueRegression[pathContInd])
                {
                    BuildNewRoom(newRoom, pathContInd++);
                }
                else
                {
                    BuildNewRoom(rooms[Random.Range(0, rooms.Count)], 0);
                }
            }
            else if (!bossRoom)
            {
                bossRoom = true;
                BuildNewRoom(rooms.Single(r => r.i == maxX && r.j == maxY), 0);
            }
        }

        private RoomInfo GetNewRoom()
        {
            RoomInfo roomToInstantiate = roomPool.GetRoom();
            while (rooms.Count(r => r.roomInfo.Equals(roomToInstantiate)) == roomToInstantiate.maxNumberOfRooms)
            {
                roomToInstantiate = roomPool.GetRoom();
            }
            return roomToInstantiate;
        }

        private void GetRandomDirection(ref int x, ref int y)
        {
            int dir = Random.Range(0, 4);
            if (dir == 0)
            {
                x = 1;
                y = 0;
            }
            else if (dir == 1)
            {
                x = -1;
                y = 0;
            }
            else if (dir == 2)
            {
                x = 0;
                y = 1;
            }
            else if (dir == 3)
            {
                x = 0;
                y = -1;
            }
        }

        private void EnableDoors(Room room)
        {
            List<Direction> normalDoors = new List<Direction>();
            List<Direction> keyDoors = new List<Direction>();
            Room roomToEvaluate = rooms.FirstOrDefault(r => r.i == room.i + 1 && r.j == room.j);
            if (roomToEvaluate != null)
            {
                if (roomToEvaluate.roomInfo.needKey)
                    keyDoors.Add(Direction.Right);
                else
                    normalDoors.Add(Direction.Right);
            }
            roomToEvaluate = rooms.FirstOrDefault(r => r.i == room.i - 1 && r.j == room.j);
            if (roomToEvaluate != null)
            {
                if (roomToEvaluate.roomInfo.needKey)
                    keyDoors.Add(Direction.Left);
                else
                    normalDoors.Add(Direction.Left);
            }
            roomToEvaluate = rooms.FirstOrDefault(r => r.i == room.i && r.j == room.j + 1);
            if (roomToEvaluate != null)
            {
                if (roomToEvaluate.roomInfo.needKey)
                    keyDoors.Add(Direction.Up);
                else
                    normalDoors.Add(Direction.Up);
            }
            roomToEvaluate = rooms.FirstOrDefault(r => r.i == room.i && r.j == room.j - 1);
            if (roomToEvaluate != null)
            {
                if (roomToEvaluate.roomInfo.needKey)
                    keyDoors.Add(Direction.Down);
                else
                    normalDoors.Add(Direction.Down);
            }
            room.gameObject.GetComponent<RoomManager>().ActiveDoors(normalDoors, keyDoors);
        }

        private void OnCreated()
        {
            foreach (var item in rooms)
            {
                RoomManager room = item.gameObject.GetComponent<RoomManager>();
                switch (room.GetRoomType())
                {
                    case RoomType.EnemyRoom:
                        EnemyRoomManager enemyRoom = room as EnemyRoomManager;
                        int numEnemies = Random.Range(0, 3);
                        List<EnemyInfo> enemies = new List<EnemyInfo>();
                        for (int i = 0; i < numEnemies; i++)
                        {
                            EnemyInfo enemy = roomPool.enemyPool.GetRandomEnemy();
                            while (enemy.hasMaxNumPerRoom && enemyRoom.enemiesGenerated.Count(e => e.gameObject.name.Equals(enemy.gameObject.name)) == enemy.maxNumPerRoom)
                            {
                                enemy = roomPool.enemyPool.GetRandomEnemy();
                            }
                            enemyRoom.enemiesGenerated.Add(enemy);
                        }
                        break;
                    case RoomType.InteractionRoom:
                        break;
                    case RoomType.BossRoom:
                        break;
                    case RoomType.initialRoom:
                        break;
                }
                room.OnCreate();
            }
        }
    }

    public class Room
        {
            public Room(RoomInfo roomInfo, int i, int j)
            {
                this.roomInfo = roomInfo;
                this.i = i;
                this.j = j;
            }
            public GameObject gameObject;
            public RoomInfo roomInfo;
            public int i;
            public int j;
        }

        public enum Direction
        {
            Up,
            Down,
            Right,
            Left,
            None
        }

}
