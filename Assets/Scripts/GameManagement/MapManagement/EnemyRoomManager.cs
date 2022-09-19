using Combat;
using System.Collections.Generic;
using UnityEngine;

namespace FloorManagement
{
    public class EnemyRoomManager : RoomManager
    {
        public GameObject[] enemySlots = new GameObject[4];
        [HideInInspector]
        public List<GameObject> enemies = new List<GameObject>();
        [HideInInspector]
        public List<EnemyInfo> enemiesGenerated = new List<EnemyInfo>();
        public GameObject bossSlot;

        public override RoomType GetRoomType()
        {
            return RoomType.EnemyRoom;
        }

        override public void OnCreate()
        {
            int count = 0;
            foreach (var enemy in enemiesGenerated)
            {
                enemies.Add(Instantiate(enemy.gameObject, enemySlots[count].transform.position, Quaternion.identity, transform));
                count++;
            }
        }

        public override void EnterOnRoom(GameObject player)
        {
            base.EnterOnRoom(player);
            if (enemies.Count > 0)
                CombatManager.combatManager.StartCombat(enemies);
        }
    }
}

