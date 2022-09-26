using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace MapManagement.RoomManagement
{
    public abstract class RoomManager : MonoBehaviour
    {
        public Door[] doors = new Door[4] { new Door(Direction.Up), new Door(Direction.Down) , new Door(Direction.Left) , new Door(Direction.Right) };
        public GameObject playerSlot;
        
        private List<DoorInfo> activeDoors = new List<DoorInfo>();

        public virtual void EnterOnRoom(GameObject player)
        {
            player.transform.position = playerSlot.transform.position;
        }

        abstract public void OnCreate();

        public Direction GetDirectionOfDoor(GameObject doorSelected)
        {
            foreach (var door in doors)
            {
                if (door.doorOpen.gameObject.GetInstanceID().Equals(doorSelected.GetInstanceID()))
                {
                    return door.dir;
                }
            }
            return Direction.None;
        }


        public void ActiveDoors(List<Direction> normalDoors, List<Direction> keyDoors)
        {
            foreach (var door in doors)
            {
                if (normalDoors.Contains(door.dir))
                {
                    activeDoors.Add(new DoorInfo(door, false));
                } 
                else if (keyDoors.Contains(door.dir))
                {
                    activeDoors.Add(new DoorInfo(door, true));
                }
            }
            foreach (var door in activeDoors)
            {
                OpenDoor(door);
            }
        }

        public void OpenAllDoors()
        {
            foreach (var door in activeDoors)
            {
                if (door.keyDoor)
                {
                    // Change door closed to door closed with key
                    if (door.hasBeenOpened)
                    {
                        OpenDoor(door);
                    }
                    else
                    {
                        CloseDoor(door);
                    }
                }
                else
                {
                    OpenDoor(door);
                }
            }
        }

        public void CloseAllDoors()
        {
            foreach (var door in activeDoors)
            {
                CloseDoor(door);
            }
        }

        private void OpenDoor(DoorInfo doorInfo)
        {
            doorInfo.door.doorOpen.SetActive(true);
            doorInfo.door.doorClosed.SetActive(false);

        }

        private void CloseDoor(DoorInfo doorInfo)
        {
            doorInfo.door.doorOpen.SetActive(false);
            doorInfo.door.doorClosed.SetActive(true);
        }

        private void OpenKeyDoor(DoorInfo doorInfo)
        {
            
        }


        public abstract RoomType GetRoomType();


        private class DoorInfo
        {

            public DoorInfo(Door door, bool keyDoor)
            {
                this.door = door;
                this.keyDoor = keyDoor;
                this.hasBeenOpened = false;
            }
            public Door door;
            public bool keyDoor;
            public bool hasBeenOpened;
        }
    }
   

    [System.Serializable]
    public class Door
    {
        public Door(Direction direction)
        {
            dir = direction;
        }

        [ReadOnly]
        public Direction dir;
        public GameObject doorOpen;
        public GameObject doorClosed;
        public GameObject doorClosedWithKey;
    }


    public enum RoomType
    {
        EnemyRoom,
        InteractionRoom,
        BossRoom,
        initialRoom
    }
}
