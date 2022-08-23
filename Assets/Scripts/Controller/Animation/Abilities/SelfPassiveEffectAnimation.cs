using Combat;
using GameManagement;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animations
{
    public class SelfPassiveEffectAnimation : IPassiveSpellAnimation
    {
        [LabelWidth(120)]
        [SerializeField] GameObject particles;

        public void PlaySpellAnimation(CombatSignalData passiveData, List<SignalData> signalDatas)
        {
            passiveData.user.GetComponent<TurnCombat>().StartCoroutine(StartSpellAnimation(passiveData, signalDatas));
        }

        private IEnumerator StartSpellAnimation(CombatSignalData passiveData, List<SignalData> signalDatas)
        {
            GameObject newParticles = Object.Instantiate(particles, passiveData.user.transform.position, Quaternion.identity);
            float time = newParticles.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length;
            yield return new WaitForSeconds(time);
            Object.Destroy(newParticles);
            UI.UIManager.manager.SendData(signalDatas);
        }
    }
}