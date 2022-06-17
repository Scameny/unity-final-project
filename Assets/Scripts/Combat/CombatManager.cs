using Combat;
using Character.Character;
using Items;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using GameManagement;
using Sirenix.OdinInspector;

public class CombatManager : MonoBehaviour
{

    public static CombatManager combatManager;

    [Header("Debug")]
    public List<GameObject> enemiesForTest = new List<GameObject>();

    protected List<GameObject> enemies = new List<GameObject>();
    protected List<GameObject> charactersInCombat = new List<GameObject>();

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
        this.enemies = enemies;
        charactersInCombat.AddRange(enemies);
        charactersInCombat.Add(player.gameObject);
        foreach (GameObject character in charactersInCombat)
        {
            character.GetComponent<TurnCombat>().enabled = true;
        }
    }


    public void EndCombat()
    {
        player.GetComponent<TurnCombat>().enabled = false;
        charactersInCombat.Clear();
        ((Hero)player.GetCharacter()).AddExp(expStored);
        ((Hero)player.GetCharacter()).AddItems(itemsStored);
        GameManager.gm.EndCombat();
    }


    public void PauseCombat()
    {
        foreach (GameObject character in charactersInCombat)
        {
            character.GetComponent<TurnCombat>().TurnPreparationPause();
        }
        combatActive = false;
    }

    public void ResumeCombat()
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy.activeInHierarchy)
                enemy.GetComponent<EnemyCombat>().TurnPreparationResume();
        }
        combatActive = true;
    }
    #endregion

    #region Characters operations
    public void EnemyDeath(EnemyCombat enemy)
    {
        expStored += ((Npc)enemy.GetCharacter()).GetRewardExp();
        itemsStored.AddRange(((Npc)enemy.GetCharacter()).GetRewardItems());
        enemies.Remove(enemy.gameObject);
        if (enemies.Count == 0)
        {
            EndCombat();
        }
        enemy.gameObject.SetActive(false);
    }

    public void HeroDeath()
    {
        EndCombat();
        foreach (var npc in enemies)
        {
            npc.SetActive(false);
        }
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
            enemy.GetComponent<TurnCombat>().GetCharacter().Heal(999.0f);
        }
    }
    #endregion



}
