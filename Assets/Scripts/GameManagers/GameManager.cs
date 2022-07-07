using FloorManagement;
using GameControl;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace GameManagement
{
    public class GameManager : MonoBehaviour
    {
        // current floor

        public RoomPool roomPoolToTest;
        public float combatTurnWait { private set; get; } = 0.1f;


        public static GameManager gm;


        FloorGenerator floorGenerator;
        GameObject player;
        RoomManager currentRoom;
        bool isInCombat = false;

        private void Awake()
        {
            gm = this;
        }
        private void Start()
        {
            floorGenerator = GetComponent<FloorGenerator>();
            player = GameObject.FindGameObjectWithTag("Player");
        }


        private void Update()
        {
            if (!isInCombat)
            {
                Direction dir = player.GetComponent<PlayerControllerPC>().SelectDoor(currentRoom);
                if (!dir.Equals(Direction.None))
                {
                    GoToRoom(dir);
                }
            }
        }

        private void GoToRoom(Direction dir)
        {
            currentRoom = floorGenerator.GetRoom(currentRoom, dir);
            int x = 0;
            int y = 0;
            UtilsClass.instance.GetDirection(dir, ref x, ref y);
            Camera.main.transform.position = Camera.main.transform.position + new Vector3(18 * x, 10 * y, 0);
            currentRoom.EnterOnRoom(player);
        }

        public void EndCombat()
        {
            isInCombat = false;
        }

        [Button]
        public void StartGame()
        {
            floorGenerator.GenerateFloor(8, roomPoolToTest);
            currentRoom = floorGenerator.GetBaseRoom();
        }

        public void SetInCombat(bool isInCombat)
        {
            this.isInCombat = isInCombat;
        }
    }
}


