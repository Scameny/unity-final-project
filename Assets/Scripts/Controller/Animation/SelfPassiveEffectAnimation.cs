using Abilities.Passive;
using Combat;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;

namespace Animations
{
    public class SelfPassiveEffectAnimation : IPassiveSpellAnimation
    {
        [LabelWidth(120)]
        [SerializeField] GameObject particles;

        public void PlaySpellAnimation(PassiveData passiveData, Action<PassiveData> function)
        {
            passiveData.user.GetComponent<TurnCombat>().StartCoroutine(StartSpellAnimation(passiveData, function));
        }

        private IEnumerator StartSpellAnimation(PassiveData passiveData, Action<PassiveData> function)
        {
            GameObject newParticles = GameObject.Instantiate(particles, passiveData.user.transform.position, Quaternion.identity);
            float time = newParticles.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length;
            yield return new WaitForSeconds(time);
            GameObject.Destroy(newParticles);
            function(passiveData);
        }
    }
}