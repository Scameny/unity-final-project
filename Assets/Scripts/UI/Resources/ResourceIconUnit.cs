using Character.Character;
using Character.Stats;
using GameManagement;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ResourceIconUnit : MonoBehaviour, IObserver<SignalData>
    {
        public ResourceType resourceType;

        TextMeshProUGUI textElement;
        Image icon;
        DefaultCharacter player;

        public void OnCompleted()
        {

        }

        public void OnError(Exception error)
        {
            Debug.LogError("Error on icon resource unit of resource type " + resourceType.ToString() + ": " + error.Message);
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
            textElement = GetComponentInChildren<TextMeshProUGUI>();
            icon = GetComponentInChildren<Image>();
        }
        private void Update()
        {
            UpdateResourceUnit();
        }

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<DefaultCharacter>();
        }

        private void UpdateResourceUnit()
        {
            if (player.GetCurrentResource(resourceType) == 0)
            {
                textElement.gameObject.SetActive(false);
                icon.gameObject.SetActive(false);
            }
            else
            {
                textElement.gameObject.SetActive(true);
                icon.gameObject.SetActive(true);
                textElement.text = player.GetCurrentResource(resourceType).ToString();
            }
        }
    }
}