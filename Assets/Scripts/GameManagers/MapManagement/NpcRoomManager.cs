using DialogueEditor;
using System.Collections.Generic;
using UI;
using UnityEngine;

namespace FloorManagement
{
    public class NpcRoomManager : RoomManager
    {
        public List<GameObject> interactableObjects = new List<GameObject>();


        public override void EnterOnRoom(GameObject player)
        {
            base.EnterOnRoom(player);
            foreach (var item in interactableObjects)
            {
                item.GetComponentInChildren<CharacterUI>().EnableSelector(true);
            }
        }

        public override RoomType GetRoomType()
        {
            return RoomType.InteractionRoom;
        }

        public override void OnCreate()
        {
            throw new System.NotImplementedException();
        }
    }
}
