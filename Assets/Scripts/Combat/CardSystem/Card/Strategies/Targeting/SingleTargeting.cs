using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameControl;
using System;
using Combat;
using UI;
using GameManagement;

namespace Strategies.TargetingStrategies
{
    public class SingleTargeting : TargetingStrategy
    {
        public override void AbilityTargeting(GameObject user, IEnumerable<GameObject> enemies, Action<IEnumerable<GameObject>, bool> effectAction)
        {
            HeroCombat heroCombat = user.GetComponent<HeroCombat>();
            heroCombat.StartCoroutine(Targeting(user, enemies, effectAction));
        }

        private IEnumerator Targeting(GameObject user, IEnumerable<GameObject> enemies, Action<IEnumerable<GameObject>, bool> effectAction)
        {
            PlayerControllerPC playerController = user.GetComponent<PlayerControllerPC>();
            List<GameObject> toRet = new List<GameObject>();

            UIManager.manager.SendData(new UISignalData(GameSignal.ENABLE_UI_ELEMENT, UIElement.COMBAT_FRAME, false));
            UIManager.manager.ChangeSceneToSelection(enemies, true);
            while (true)
            {
                if (playerController.CancelAction())
                {
                    effectAction.Invoke(toRet, false);
                    break;
                }
                else
                {
                    GameObject characterSelected = playerController.SelectCharacter();
                    if (characterSelected != null)
                    {
                        toRet.Add(characterSelected);
                        effectAction.Invoke(toRet, true);
                        break;
                    }
                    yield return null;
                }
            }
            UIManager.manager.SendData(new UISignalData(GameSignal.ENABLE_UI_ELEMENT, UIElement.COMBAT_FRAME, true));
            UIManager.manager.ChangeSceneToSelection(enemies, false);
        }

    }
}

