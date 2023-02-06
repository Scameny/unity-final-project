using System;
using System.Collections;
using UnityEngine;

namespace Animations.Character
{
    public class CharacterAnimation : MonoBehaviour
    {

        Animator animator;

        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
        }

        public void Die()
        {
            AnimationQueue.Instance.AddAnimationToQueue(DieCoroutine(AnimationQueue.Instance.EndAnimation));
        }

        private IEnumerator DieCoroutine(Action endAnimation)
        {
            animator.Play("Die");
            float time = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
            yield return time;
            endAnimation();
            gameObject.SetActive(false);
        }

        public void Hurt()
        {
            AnimationQueue.Instance.AddAnimationToQueue(HurtCoroutine(AnimationQueue.Instance.EndAnimation));
        }

        private IEnumerator HurtCoroutine(Action endAnimation)
        {
            animator.Play("Hurt");
            float time = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
            yield return time;
            endAnimation();
        }


    }

}