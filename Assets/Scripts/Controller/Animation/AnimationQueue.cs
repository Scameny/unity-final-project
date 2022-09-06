using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animations
{
    public class AnimationQueue : MonoBehaviour
    {
        public static AnimationQueue Instance;

        Queue<IEnumerator> queue = new Queue<IEnumerator>();
        Coroutine animationCoroutine;

        private void Awake()
        {
            Instance = this;
        }


        private void Update()
        {
            if (queue.Count > 0 && animationCoroutine == null)
            {
                StartAnimation();
            }
        }
  
        private void StartAnimation()
        {
            animationCoroutine = StartCoroutine(queue.Dequeue());
        }

        public void AddAnimationToQueue(IEnumerator animation)
        {
            queue.Enqueue(animation);
        }
   
        public void EndAnimation()
        {
            animationCoroutine = null;
        }

        public bool DoingAnimations()
        {
            return (queue.Count == 0 && animationCoroutine == null);
        }
    }

}