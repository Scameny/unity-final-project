using Character.Character;
using Character.Stats;
using GameManagement;
using System;
using System.Collections;
using UnityEngine;

namespace UI 
{
    public class CharacterUI : MonoBehaviour, IObserver<SignalData>
    {
        [SerializeField] GameObject floatingTextPrefab;

        DefaultCharacter character;
        GameObject selector;
        float timeToSpawnNextFloatingText = 0;
        

        virtual protected void Awake()
        {
            selector = transform.Find("Selector").gameObject;
            character = GetComponentInParent<DefaultCharacter>();
        }

        private void Start()
        {
            UIManager.manager.Subscribe(this);
        }

        virtual protected void Update()
        {
            if (timeToSpawnNextFloatingText > 0)
                timeToSpawnNextFloatingText -= Time.deltaTime;
            else if (timeToSpawnNextFloatingText < 0)
                timeToSpawnNextFloatingText = 0;
        }

        public void EnableSelector(bool enable)
        {
            selector.SetActive(enable);
        }

        private IEnumerator CreateFloatingText(ResourceType resourceType, int value, GameObject user, float timeToWait)
        {
            timeToSpawnNextFloatingText = floatingTextPrefab.GetComponent<FloatingText>().timeBetweenText;
            yield return new WaitForSeconds(timeToWait);
            GameObject floatingText = Instantiate(floatingTextPrefab, user.transform.position, Quaternion.identity, transform);
            floatingText.GetComponent<FloatingText>().SetValues(resourceType, value);
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            Debug.LogError("Character UI has an error:" + error.Message);
        }

        public void OnNext(SignalData signalData)
        {
            if (signalData.signal.Equals(GameSignal.RESOURCE_MODIFY) && (signalData as ResourceSignalData).user.GetInstanceID().Equals(character.gameObject.GetInstanceID()))
            {
                ResourceSignalData resourceSignalData = signalData as ResourceSignalData;
                StartCoroutine(CreateFloatingText(resourceSignalData.resourceType, resourceSignalData.resourceAmount, resourceSignalData.user, timeToSpawnNextFloatingText));
            }
        }

        public DefaultCharacter GetCharacter()
        {
            return character;
        }
    }
}
