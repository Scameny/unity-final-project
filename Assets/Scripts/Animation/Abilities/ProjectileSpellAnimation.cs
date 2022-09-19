using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using System.Collections;
using GameManagement;

namespace Animations.Ability
{
    [Serializable]
    public class ProjectileSpellAnimation : ISpellAnimation
    {
        [SerializeField] GameObject projectile;
        [SerializeField] float speed;

        public void PlaySpellAnimation(GameObject user, IEnumerable<GameObject> targets, List<SignalData> signalDatas)
        {
            AnimationQueue.Instance.AddAnimationToQueue(StartSpellAnimation(user, targets, signalDatas, AnimationQueue.Instance.EndAnimation));
        }

        protected IEnumerator StartSpellAnimation(GameObject user, IEnumerable<GameObject> targets, List<SignalData> signalDatas, Action endAnimation)
        {
           foreach (var item in targets)
           {
                GameObject newProj = GameObject.Instantiate(projectile, user.transform.position, Quaternion.identity);
                yield return null;
                newProj.transform.DOMove(item.transform.position, speed).OnComplete(() =>
                {
                    newProj.GetComponent<Animator>().Play("End");
                    UnityEngine.Object.Destroy(newProj, newProj.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length);
                    
                    UI.UIManager.manager.SendData(signalDatas);
                    endAnimation();
                });
           }
        }
    }

}
