using Combat;
using FloorManagement;
using GameManagement;
using Interaction;
using UnityEngine;

namespace GameControl 
{
    public class PlayerControllerPC : MonoBehaviour
    {

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (GameManager.gm.GetCurrentState().Equals(GameState.Moving))
                {
                    MoveToAnotherRoom();
                    SelectInteractionNpc();
                }
            }
        }

        public GameObject SelectCharacter()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Collider2D col = Physics2D.OverlapPoint(pos);
                if (col != null && col.GetComponentInParent<TurnCombat>() != null)
                {
                    return col.GetComponentInParent<TurnCombat>().gameObject;
                }
            }
            return null;
        }


        private void MoveToAnotherRoom()
        {    
            Direction dir = SelectDirection(GameManager.gm.GetCurrentRoom());
            if (!dir.Equals(Direction.None))
            {
                GameManager.gm.GoToRoom(dir);
            }
        }

        private void SelectInteractionNpc()
        {
            
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D col = Physics2D.OverlapPoint(pos);
            if (col != null && col.GetComponentInParent<NPCInteractable>() != null)
            {
                col.GetComponentInParent<NPCInteractable>().StartConversation();
            }
        }

        public bool CancelAction()
        {
            return Input.GetKeyDown(KeyCode.Escape);
        }

        private Direction SelectDirection(RoomManager room)
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D col = Physics2D.OverlapPoint(pos);
            Direction dir = Direction.None;
            if (col != null)
            {
                dir = room.GetDirectionOfDoor(col.gameObject);
            }
            return dir;
        }
    }

}