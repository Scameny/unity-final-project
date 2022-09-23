using Animations;
using Combat;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Combat
{
    public class UIEndTurnButton : MonoBehaviour
    {
        HeroCombat heroCombat;
        Button button;

        void Start()
        {
            heroCombat = GameObject.FindGameObjectWithTag("Player").GetComponent<HeroCombat>();
            button = GetComponent<Button>();
            GetComponent<Button>().onClick.AddListener(() =>
            {
                AnimationQueue.Instance.AddAnimationToQueue(EndTurnCoroutine()); ;
                button.interactable = false;
            });
        }

        private IEnumerator EndTurnCoroutine()
        {
            heroCombat.EndOfTurn();
            yield return null;
            AnimationQueue.Instance.EndAnimation();
        }
    }

}
