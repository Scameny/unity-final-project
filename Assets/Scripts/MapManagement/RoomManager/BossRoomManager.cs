using Combat;
using System.Collections.Generic;
using UnityEngine;

namespace MapManagement.RoomManagement
{
    public class BossRoomManager : RoomManager
    {
        [SerializeField] GameObject bossSlot;

        GameObject boss;

        override public void OnCreate()
        {
            boss = Instantiate(boss, bossSlot.transform.position, Quaternion.identity, transform);
        }

        public override RoomType GetRoomType()
        {
            return RoomType.BossRoom;
        }

        public void SetBossToGenerate(GameObject boss)
        {
            this.boss = boss;
        }

        public override void EnterOnRoom(GameObject player)
        {
            base.EnterOnRoom(player);
            CombatManager.combatManager.StartCombat(new List<GameObject>() { boss });
        }

    }

}