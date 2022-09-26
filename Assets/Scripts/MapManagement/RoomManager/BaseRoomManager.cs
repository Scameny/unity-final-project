
using UnityEngine;

namespace MapManagement.RoomManagement
{
    public class BaseRoomManager : RoomManager
    {
        public override void EnterOnRoom(GameObject player)
        {
            base.EnterOnRoom(player);
        }

        public override RoomType GetRoomType()
        {
            return RoomType.initialRoom;
        }

        public override void OnCreate()
        {

        }
    }

}
