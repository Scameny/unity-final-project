using Combat;
using GameManagement;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animations
{
    [Serializable]
    public class TargetSpellAnimation : ISpellAnimation
    {
        [LabelWidth(120)]
        [SerializeField] GameObject particles;

        public void PlaySpellAnimation(GameObject user, IEnumerable<GameObject> targets, List<SignalData> signalDatas)
        {
            user.GetComponent<TurnCombat>().StartCoroutine(StartSpellAnimation(user, targets, signalDatas));
        }

        private IEnumerator StartSpellAnimation(GameObject user, IEnumerable<GameObject> targets, List<SignalData> signalDatas)
        {
            foreach (var item in targets)
            {
                GameObject newParticles = UnityEngine.Object.Instantiate(particles, item.transform.position, Quaternion.identity);
                float time = newParticles.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length;
                yield return new WaitForSeconds(time);
                UnityEngine.Object.Destroy(newParticles);
                UI.UIManager.manager.SendData(signalDatas);
            }
        }
    }
}

