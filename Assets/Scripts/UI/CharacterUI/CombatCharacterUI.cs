using Character.Buff;
using Character.Character;
using Character.Stats;
using GameManagement;
using System.Collections;
using UnityEngine;

namespace UI
{
    public class CombatCharacterUI : CharacterUI
    {
        GameObject floatingTextPrefab;
        DefaultCharacter character;
        float timeToSpawnNextFloatingText = 0;

        // Start is called before the first frame update
        protected override void Start()
        {
            character = GetComponentInParent<DefaultCharacter>();
            floatingTextPrefab = Resources.Load<GameObject>("UI/FloatingText");
            base.Start();
        }

        virtual protected void Update()
        {
            if (timeToSpawnNextFloatingText > 0)
                timeToSpawnNextFloatingText -= Time.deltaTime;
            else if (timeToSpawnNextFloatingText < 0)
                timeToSpawnNextFloatingText = 0;
        }


        private IEnumerator CreateResourceFloatingText(ResourceType resourceType, int value, GameObject user, float timeToWait)
        {
            timeToSpawnNextFloatingText += floatingTextPrefab.GetComponent<FloatingText>().GetTimeBetweenText();
            yield return new WaitForSeconds(timeToWait);
            GameObject floatingText = Instantiate(floatingTextPrefab, user.transform.position, Quaternion.identity, transform);
            floatingText.GetComponent<FloatingText>().SetValues(resourceType, value);
        }

        private IEnumerator CreateBuffFloatingText(BaseBuff buff, GameSignal gameSignal, GameObject user, float timeToWait)
        {
            timeToSpawnNextFloatingText += floatingTextPrefab.GetComponent<FloatingText>().GetTimeBetweenText();
            yield return new WaitForSeconds(timeToWait);
            GameObject floatingText = Instantiate(floatingTextPrefab, user.transform.position, Quaternion.identity, transform);
            floatingText.GetComponent<FloatingText>().SetValues(buff, gameSignal);
        }



        override public void OnNext(SignalData signalData)
        {
            base.OnNext(signalData);
            if (signalData.signal.Equals(GameSignal.RESOURCE_MODIFY) && signalData.GetType().Equals(typeof(CombatResourceSignalData)) && 
                (signalData as CombatResourceSignalData).user.Equals(character.gameObject))
            {
                CombatResourceSignalData resourceSignalData = signalData as CombatResourceSignalData;
                StartCoroutine(CreateResourceFloatingText(resourceSignalData.resourceType, resourceSignalData.resourceAmount, resourceSignalData.user, timeToSpawnNextFloatingText));
            }
            else if ((signalData.signal.Equals(GameSignal.NEW_TRAIT) || signalData.signal.Equals(GameSignal.TRAIT_RENEWED) || signalData.signal.Equals(GameSignal.TRAIT_EXPIRED) || signalData.signal.Equals(GameSignal.REMOVE_TRAIT)) && (signalData as TraitSignalData).user.Equals(character.gameObject))
            {
                TraitSignalData traitSignalData = signalData as TraitSignalData;
                StartCoroutine(CreateBuffFloatingText(traitSignalData.trait, traitSignalData.signal,traitSignalData.user, timeToSpawnNextFloatingText));
            }
        }

        public DefaultCharacter GetCharacter()
        {
            return character;
        }
    }

}
