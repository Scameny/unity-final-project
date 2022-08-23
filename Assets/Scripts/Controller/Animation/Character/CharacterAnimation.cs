using GameManagement;
using System;
using UnityEngine;

namespace Animations.Character
{

    public class CharacterAnimation : MonoBehaviour, IObserver<SignalData>
    {

        Animator animator;
        IDisposable disposable;

        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
            disposable = UI.UIManager.manager.Subscribe(this);
        }

        public void OnCompleted()
        {
            disposable.Dispose();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(SignalData value)
        {
            if (value.signal.Equals(GameSignal.CHARACTER_DIE) && (value as CombatSignalData).user.Equals(gameObject))
            {
                animator.Play("Die");
                gameObject.SetActive(false);
            }
            if (value.signal.Equals(GameSignal.DAMAGE_RECEIVED) && (value as CombatSignalData).user.Equals(gameObject))
            {
                animator.Play("Hurt");
            }

        }

      
    }

}