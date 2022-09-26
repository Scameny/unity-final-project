using System.Collections.Generic;
using UI;
using UnityEngine;
using Utils;

namespace MapManagement.RoomManagement
{
    public class NpcRoomManager : RoomManager
    {
        [SerializeField] List<PoolObject<GameObject>> interactableObjectsPool = new List<PoolObject<GameObject>>();
        [SerializeField] List<GameObject> npcSlots = new List<GameObject>();
        List<GameObject> npcs = new List<GameObject>();

        public override void EnterOnRoom(GameObject player)
        {
            base.EnterOnRoom(player);
            EnableSelectors(true);
        }

        public void EnableSelectors(bool enable)
        {
            UIManager.manager.ChangeSceneToSelection(npcs, enable);
        }

        public override RoomType GetRoomType()
        {
            return RoomType.InteractionRoom;
        }

        public override void OnCreate()
        {
            List<GameObject> interactableObjectList = UtilsClass.instance.GetListFromPool(interactableObjectsPool, npcSlots.Count);
            int count = 0;
            foreach (var item in interactableObjectList)
            {
                npcs.Add(Instantiate(item, npcSlots[count].transform.position, Quaternion.identity, npcSlots[count].transform));
                count++;
            }
        }
    }
}
