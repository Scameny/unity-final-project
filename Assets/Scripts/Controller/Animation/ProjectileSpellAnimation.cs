using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using System.Collections;
using Combat;

namespace Animations
{
    [Serializable]
    public class ProjectileSpellAnimation : ISpellAnimation
    {
        [SerializeField] GameObject projectile;
        [SerializeField] float speed;

        public void PlaySpellAnimation(GameObject user, IEnumerable<GameObject> targets, Action<GameObject, IEnumerable<GameObject>> function)
        {
            user.GetComponent<TurnCombat>().StartCoroutine(StartSpellAnimation(user, targets, function));
        }

        private IEnumerator StartSpellAnimation(GameObject user, IEnumerable<GameObject> targets, Action<GameObject, IEnumerable<GameObject>> function)
        {
           foreach (var item in targets)
           {
                GameObject newProj = GameObject.Instantiate(projectile, user.transform.position, Quaternion.identity);
                yield return null;
                newProj.transform.DOMove(item.transform.position, speed).OnComplete(() =>
                {
                    newProj.GetComponent<Animator>().Play("End");
                    GameObject.Destroy(newProj, newProj.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length);
                    function(user, targets);
                });
           }
        }
    }

}
