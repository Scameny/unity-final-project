using GameManagemenet.GameConfiguration;
using MapManagement;
using MapManagement.RoomManagement;
using Sirenix.OdinInspector;
using UI;
using UnityEngine;
using Utils;

namespace GameManagement
{
    public class GameManager : MonoBehaviour
    {
        // current floor

        [SerializeField] GamePreferences preferences;
        public RoomPool roomPoolToTest;
        public float combatTurnWait { private set; get; } = 0.1f;


        public static GameManager gm;

        private GameState currentGameState;

        FloorGenerator floorGenerator;
        GameObject player;
        RoomManager currentRoom;

        private void Awake()
        {
            gm = this;
        }

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            floorGenerator = GetComponent<FloorGenerator>();
            StartGame();
        }



        #region Room operations

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

        public RoomManager GetCurrentRoom()
        {
            return currentRoom;
        }
        #endregion

        #region GameState operations

        private void EndGame()
        {
            OpenMenu();
            UIManager.manager.SendData(new SignalData(GameSignal.END_GAME));
        }

        [Button]
        public void StartGame()
        {
            floorGenerator.GenerateFloor(8, roomPoolToTest);
            currentGameState = GameState.Moving;
            currentRoom = floorGenerator.GetBaseRoom();
            currentRoom.EnterOnRoom(player);
            UIManager.manager.SendData(new SignalData(GameSignal.START_GAME));
        }


        public void EndCombat()
        {
            currentGameState = GameState.Moving;
            if (currentRoom.GetRoomType().Equals(RoomType.BossRoom))
            {
                EndGame();
            }
        }

        public void StartCombat()
        {
            currentGameState = GameState.Combat;
        }

        public void OpenMenu()
        {
            currentGameState = GameState.OnGameMenu;
        }

        public void CloseMenu()
        {
            currentGameState = GameState.Moving;
        }

        public void StartInteraction()
        {
            currentGameState = GameState.Interacting;
        }

        public void EndInteraction()
        {
            currentGameState = GameState.Moving;
        }

        public GameState GetCurrentState()
        {
            return currentGameState;
        }
        #endregion
    }

    public enum GameState
    {
        Combat, Interacting, Moving, OnGameMenu, OnStartingMenu
    }
}


