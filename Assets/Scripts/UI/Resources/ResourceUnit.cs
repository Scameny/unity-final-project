using Character.Character;
using Character.Stats;
using GameManagement;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ResourceUnit : MonoBehaviour, IObserver<SignalData>
    {
        public ResourceType resourceType;

        Slider resourceSlider;
        TextMeshProUGUI textElement;
        DefaultCharacter player;

        public void OnCompleted()
        {

        }

        public void OnError(Exception error)
        {
            Debug.LogError("Error on resource unit of resource type " + resourceType.ToString() + ": " + error.Message);
        }

        public void OnNext(SignalData value)
        {
            if (value.signal.Equals(GameSignal.ENABLE_UI_ELEMENT) && (value as UISignalData).element.Equals(UIElement.RESOURCES_FRAME))
            {
                gameObject.SetActive((value as UISignalData).enable);
            }
        }

        private void Awake()
        {
            resourceSlider = GetComponentInChildren<Slider>();
            textElement = transform.Find("ResourceUnit").GetComponentInChildren<TextMeshProUGUI>();
        }

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<DefaultCharacter>();
        }

        private void Update()
        {
            UpdateResourceUnit();
        }

        private void UpdateResourceUnit()
        {
            resourceSlider.maxValue = player.GetMaxValueOfResource(resourceType);
            resourceSlider.value = player.GetCurrentResource(resourceType);
            textElement.text = resourceSlider.value.ToString() + "/" + resourceSlider.maxValue.ToString();
        }
    }

}
