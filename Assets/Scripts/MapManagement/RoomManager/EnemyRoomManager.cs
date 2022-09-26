using Combat;
using System.Collections.Generic;
using UnityEngine;

namespace MapManagement.RoomManagement
{
    public class EnemyRoomManager : RoomManager
    {
        [SerializeField] GameObject[] enemySlots = new GameObject[4];
        List<GameObject> enemies = new List<GameObject>();


        public override RoomType GetRoomType()
        {
            return RoomType.EnemyRoom;
        }

        override public void OnCreate()
        {
            int count = 0;
            List<GameObject> enemiesToGenerate = new List<GameObject>(enemies);
            enemies.Clear();
            foreach (var enemy in enemiesToGenerate)
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

        public void SetEnemies(List<GameObject> enemies)
        {
            this.enemies = enemies;
        }

        public List<GameObject> GetEnemies()
        {
            return enemies;

        }
    }
}

