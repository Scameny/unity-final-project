using Combat;
using Character.Character;
using Items;
using NaughtyAttributes;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class CombatManager : MonoBehaviour
{

    public static CombatManager combatManager;
    public Button pauseButton, resumeButton;
    [HideInInspector]
    public List<GameObject> enemies = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> charactersInCombat = new List<GameObject>();
    HeroCombat player;
    bool combatActive = false;
    public Item itemToAdd;

    public void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<HeroCombat>();
        combatManager = this;
    }

    private void Start()
    {
        charactersInCombat.AddRange(enemies);
        charactersInCombat.Add(player.gameObject);
    }

    [Button]
    private void RessEnemies()
    {
        foreach (var enemy in enemies)
        {
            enemy.SetActive(true);
            enemy.GetComponent<TurnCombat>().character.Heal(999.0f);
        }
    }

    [Button]
    private void AddPotionToInventory()
    {
        ((Hero)player.character).inventory.AddItem(itemToAdd, 1);
    }

    [Button]
    #region Combat management
    public void StartCombat()
    {
        foreach (GameObject character in charactersInCombat)
        {
            character.GetComponent<TurnCombat>().enabled = true;
        }
        UIManager.manager.PopulateAbilityMenu(player.character.abilitiesAvaliable, player);
        UIManager.manager.PopulateItemMenu(((Hero)player.character).inventory.GetListOfItems(ItemType.Consumable), player);
        combatActive = true;
        ResumePauseButton(false);
    }


    public void EndCombat()
    {
        player.enabled = false;
        // exp
        // loot
    }


    public void PauseCombat()
    {
        player.TurnPreparationStop();
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<EnemyCombat>().TurnPreparationStop();
        }
        combatActive = false;
        ResumePauseButton(true);
    }

    public void ResumeCombat()
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy.activeInHierarchy)
                enemy.GetComponent<EnemyCombat>().TurnPreparationResume();
        }
        combatActive = true;
        ResumePauseButton(false);
    }

    public void EnemyDeath(EnemyCombat enemy)
    {
        // loot and exp store
        enemy.gameObject.SetActive(false);
        if (enemies.Count == 0)
        {
            EndCombat();
        }
        else
        {
            enemies.Remove(enemy.gameObject);
        }
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


    #endregion

    private void ResumePauseButton(bool isPaused)
    {
        resumeButton.gameObject.SetActive(isPaused);
        pauseButton.gameObject.SetActive(!isPaused);
        if (isPaused)
        {

        }
    }




}
