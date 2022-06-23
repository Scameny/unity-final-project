using Character.Stats;
using System.Collections;
using UnityEngine;

namespace UI 
{
    public class CharacterUI : MonoBehaviour
    {
        [SerializeField] GameObject floatingTextPrefab;

        GameObject selector;
        float timeToSpawnNextFloatingText = 0;
        

        virtual protected void Awake()
        {
            selector = transform.Find("Selector").gameObject;
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

        public void ProcessModifyResourceText(ResourceType resourceType, int value, GameObject user)
        {
            StartCoroutine(CreateFloatingText(resourceType, value, user, timeToSpawnNextFloatingText));
        }

        private IEnumerator CreateFloatingText(ResourceType resourceType, int value, GameObject user, float timeToWait)
        {
            timeToSpawnNextFloatingText = floatingTextPrefab.GetComponent<FloatingText>().timeBetweenText;
            yield return new WaitForSeconds(timeToWait);
            GameObject floatingText = Instantiate(floatingTextPrefab, user.transform.position, Quaternion.identity, transform);
            floatingText.GetComponent<FloatingText>().SetValues(resourceType, value);
        }
        

    }
}
