using Character.Character;
using Items;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using GameManagement;
using Sirenix.OdinInspector;
using System.Collections;
using UI;

namespace Combat
{
    public class CombatManager : MonoBehaviour
    {

        public static CombatManager combatManager;

        [Header("Debug")]
        public List<GameObject> enemiesForTest = new List<GameObject>();

        [SerializeField] float timeToResumeCombat = 0.5f;

        protected List<GameObject> enemies = new List<GameObject>();
        protected List<GameObject> charactersInCombat = new List<GameObject>();

        bool turnPaused;
        List<Item> itemsStored = new List<Item>();
        int expStored;
        HeroCombat player;
        bool combatActive;

        public void Awake()
        {
            combatManager = this;
        }

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<HeroCombat>();
        }

        #region Combat management
        public void StartCombat(List<GameObject> enemies)
        {
            GameManager.gm.StartCombat();
            UIManager.manager.SendData(new SignalData(GameSignal.START_COMBAT));
            this.enemies = enemies;
            charactersInCombat.AddRange(enemies);
            charactersInCombat.Add(player.gameObject);
            combatActive = true;
            turnPaused = false;
            foreach (GameObject character in charactersInCombat)
            {
                character.GetComponent<TurnCombat>().StartCombat();
            }
        }


        public void EndCombat()
        {
            GameManager.gm.EndCombat();
            player.GetComponent<TurnCombat>().EndCombat();
            UIManager.manager.SendData(new SignalData(GameSignal.END_COMBAT));
            charactersInCombat.Clear();
            ((Hero)player.GetCharacter()).AddExp(expStored);
            ((Hero)player.GetCharacter()).AddItems(itemsStored);
        }


        public void PauseCombat()
        {
            turnPaused = true;
            foreach (GameObject character in charactersInCombat)
            {
                character.GetComponent<TurnCombat>().TurnPreparationPause();
            }
            combatActive = false;
        }

        public void ResumeCombat()
        {
            StartCoroutine(ResumeCombatCoroutine());
        }

        private IEnumerator ResumeCombatCoroutine()
        {
            yield return new WaitForSeconds(timeToResumeCombat);
            turnPaused = false;
            foreach (GameObject character in charactersInCombat)
            {
                if (character.activeInHierarchy)
                    character.GetComponent<TurnCombat>().TurnPreparationResume();
            }
            combatActive = true;
        }

        public bool IsTurnPaused()
        {
            return turnPaused;
        }
        #endregion

        #region Characters operations
        public void EnemyDeath(EnemyCombat enemy)
        {
            expStored += ((Npc)enemy.GetCharacter()).GetRewardExp();
            itemsStored.AddRange(((Npc)enemy.GetCharacter()).GetRewardItems());
            enemies.Remove(enemy.gameObject);
            Debug.Log("enemy death");
            enemy.GetComponent<TurnCombat>().EndCombat();
            if (enemies.Count == 0)
            {
                EndCombat();
            }
        }

        public void HeroDeath()
        {
            // Lose
            GameDebug.Instance.Log(Color.red, "You lose");
        }

        public List<GameObject> GetCharactersInCombat()
        {
            return charactersInCombat;
        }


        #endregion

        #region Debug
        [Button]
        public void StartCombat()
        {
            StartCombat(enemiesForTest);
        }

        [Button]
        private void RessEnemies()
        {
            foreach (var enemy in enemies)
            {
                enemy.SetActive(true);
                enemy.GetComponent<TurnCombat>().GetCharacter().Heal(999);
            }
        }
        #endregion

        #region Getters

        #endregion



    }

}
