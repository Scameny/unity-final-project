using GameManagement;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animations.Ability
{
    public class SelfPassiveEffectAnimation : IPassiveSpellAnimation
    {
        [LabelWidth(120)]
        [SerializeField] GameObject particles;

        public void PlaySpellAnimation(CombatSignalData passiveData, List<SignalData> signalDatas)
        {
            AnimationQueue.Instance.AddAnimationToQueue(StartSpellAnimation(passiveData, signalDatas, AnimationQueue.Instance.EndAnimation));
        }

        private IEnumerator StartSpellAnimation(CombatSignalData passiveData, List<SignalData> signalDatas, System.Action endAnimation)
        {
            GameObject newParticles = Object.Instantiate(particles, passiveData.user.transform.position, Quaternion.identity, passiveData.user.transform);
            float time = newParticles.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length;
            yield return new WaitForSeconds(time);
            Object.Destroy(newParticles);
            UI.UIManager.manager.SendData(signalDatas);
            endAnimation();
        }
    }
}