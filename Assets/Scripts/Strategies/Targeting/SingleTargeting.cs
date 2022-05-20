using Character.Abilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameControl;
using System;
using Combat;
using UI;

namespace Strategies.TargetingStrategies
{
    [CreateAssetMenu(fileName = "Targeting", menuName = "Strategy/TargetingSelection/SingleSelection", order = 1)]
    public class SingleTargeting : TargetingStrategy
    {
        public override void AbilityTargeting(GameObject user, IEnumerable<GameObject> enemies, Action<IEnumerable<GameObject>> effectAction)
        {
            HeroCombat heroCombat = user.GetComponent<HeroCombat>();
            heroCombat.StartCoroutine(Targeting(user, enemies, effectAction));
        }

        private IEnumerator Targeting(GameObject user, IEnumerable<GameObject> enemies, Action<IEnumerable<GameObject>> effectAction)
        {
            PlayerControllerPC playerController = user.GetComponent<PlayerControllerPC>();
            List<GameObject> toRet = new List<GameObject>();

            UIManager.manager.EnableSelectorInTargets(enemies, true);
            
            while (true)
            {
                if (playerController.CancelAction())
                {
                    effectAction.Invoke(toRet);
                    break;
                }
                else
                {
                    GameObject characterSelected = playerController.SelectCharacter();
                    if (characterSelected != null)
                    {
                        toRet.Add(characterSelected);
                        effectAction.Invoke(toRet);
                        break;
                    }
                    yield return null;
                }
            }

            UIManager.manager.EnableSelectorInTargets(enemies, false);
        }

    }
}
