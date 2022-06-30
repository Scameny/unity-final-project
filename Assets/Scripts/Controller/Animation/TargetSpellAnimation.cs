using Combat;
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

        public void PlaySpellAnimation(GameObject user, IEnumerable<GameObject> targets, Action<GameObject, IEnumerable<GameObject>> function)
        {
            user.GetComponent<TurnCombat>().StartCoroutine(StartSpellAnimation(user, targets, function));
        }

        private IEnumerator StartSpellAnimation(GameObject user, IEnumerable<GameObject> targets, Action<GameObject, IEnumerable<GameObject>> function)
        {
            foreach (var item in targets)
            {
                GameObject newParticles = GameObject.Instantiate(particles, item.transform.position, Quaternion.identity);
                float time = newParticles.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length;
                yield return new WaitForSeconds(time);
                GameObject.Destroy(newParticles);
                function(user, targets);
            }
        }
    }
}

