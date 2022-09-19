using FloorManagement;
using Sirenix.OdinInspector;
using UI;
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

        private GameState currentGameState;

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

        public void EnableSelectorInCurrentRoom(bool enable)
        {
            if (currentRoom.GetRoomType().Equals(RoomType.InteractionRoom))
            {
                (currentRoom as NpcRoomManager).EnableSelectors(enable);
            }
        }

        public void GoToRoom(Direction dir)
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
            currentGameState = GameState.Moving;
        }

        public void StartCombat()
        {
            currentGameState = GameState.Combat;
        }

        public void StartInteraction()
        {
            currentGameState = GameState.Interacting;
            UIManager.manager.SendData(new SignalData(GameSignal.START_INTERACTION));
        }

        public void EndInteraction()
        {
            currentGameState = GameState.Moving;
            UIManager.manager.SendData(new SignalData(GameSignal.END_INTERACTION));
        }

        [Button]
        public void StartGame()
        {
            floorGenerator.GenerateFloor(8, roomPoolToTest);
            currentGameState = GameState.Moving;
            currentRoom = floorGenerator.GetBaseRoom();
            UIManager.manager.SendData(new SignalData(GameSignal.START_GAME));
        }

        public RoomManager GetCurrentRoom()
        {
            return currentRoom;
        }

        public GameState GetCurrentState()
        {
            return currentGameState;
        }
    }

    public enum GameState
    {
        Combat, Interacting, Moving
    }
}


