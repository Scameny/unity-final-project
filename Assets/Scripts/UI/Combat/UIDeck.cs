using Animations;
using CardSystem;
using GameManagement;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Combat
{
    /// <summary>
    /// Class for all the view operations of the deck
    /// </summary>
    public class UIDeck : MonoBehaviour, IObserver<SignalData>
    {
        [SerializeField] float cardMovementDuration, timeBetweenCreatingCards;
        [SerializeField] int overlap;
        
        IDisposable disposable;
        GameObject character;
        Deck deck;

        // Start is called before the first frame update
        private void Start()
        {
            disposable = UIManager.manager.Subscribe(this);
            character = GameObject.FindGameObjectWithTag("Player");
            deck = GetComponent<Deck>();
        }

        public void OnCompleted()
        {
            disposable.Dispose();
        }

        public void OnError(Exception error)
        {
            Debug.LogError("Error on UIDeck " + error.Message + " \nstack trace: " + error.StackTrace);
        }

        public void OnNext(SignalData value)
        {

                if (value.signal.Equals(GameSignal.RECHARGE_DECK) && (value as CardContainerSignalData).container.Equals(deck))
                {
                    CardContainerSignalData data = value as CardContainerSignalData;
                    AnimationQueue.Instance.AddAnimationToQueue(InitializeDeckAnimation(data.cards));
                }
                else if (value.signal.Equals(GameSignal.DECK_INITIALIZE) && (value as CardContainerSignalData).container.Equals(deck))
                {
                    CardContainerSignalData data = value as CardContainerSignalData;
                    AnimationQueue.Instance.AddAnimationToQueue(InitializeDeckAnimation(data.cards));
                }
        }

        private IEnumerator InitializeDeckAnimation(List<Card> cards)
        {
            int count = 0;
            foreach (Card card in cards)
            {
                card.transform.SetSiblingIndex(count);
                count++;
            }
            FitDeck();
            foreach (Card card in cards)
            {
                card.GetComponent<UICombatCard>().SetCardBack(true);
                card.gameObject.SetActive(true);
                yield return new WaitForSeconds(timeBetweenCreatingCards);
            }
            AnimationQueue.Instance.EndAnimation();
        }

        [Button]
        private void FitDeck()
        {
            int count = 0;
            foreach(Transform card in transform)
            {
                card.position = transform.position + new Vector3(count * overlap, 0,0);
                count++;
            }
        }

    }

}
