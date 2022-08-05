using Character.Character;
using Character.Stats;
using GameManagement;
using System.Collections;
using UnityEngine;

namespace UI
{
    public class CombatCharacterUI : CharacterUI
    {
        [SerializeField] GameObject floatingTextPrefab;
        DefaultCharacter character;
        float timeToSpawnNextFloatingText = 0;

        // Start is called before the first frame update
        protected override void Start()
        {
            character = GetComponentInParent<DefaultCharacter>();
            base.Start();
        }

        // Update is called once per frame
        virtual protected void Update()
        {
            if (timeToSpawnNextFloatingText > 0)
                timeToSpawnNextFloatingText -= Time.deltaTime;
            else if (timeToSpawnNextFloatingText < 0)
                timeToSpawnNextFloatingText = 0;
        }

        private IEnumerator CreateFloatingText(ResourceType resourceType, int value, GameObject user, float timeToWait)
        {
            timeToSpawnNextFloatingText = floatingTextPrefab.GetComponent<FloatingText>().timeBetweenText;
            yield return new WaitForSeconds(timeToWait);
            GameObject floatingText = Instantiate(floatingTextPrefab, user.transform.position, Quaternion.identity, transform);
            floatingText.GetComponent<FloatingText>().SetValues(resourceType, value);
        }


        override public void OnNext(SignalData signalData)
        {
            base.OnNext(signalData);
            if (signalData.signal.Equals(GameSignal.RESOURCE_MODIFY) && (signalData as ResourceSignalData).user.Equals(character.gameObject))
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
