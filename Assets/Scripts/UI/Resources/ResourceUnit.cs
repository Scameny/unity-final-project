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
        IDisposable disposable;

        private void Awake()
        {
            resourceSlider = GetComponentInChildren<Slider>();
            textElement = transform.Find("ResourceUnit").GetComponentInChildren<TextMeshProUGUI>();
        }

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<DefaultCharacter>();
            disposable = UIManager.manager.Subscribe(this);
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
            if (value.signal.Equals(GameSignal.START_GAME))
            {
                resourceSlider.maxValue = player.GetMaxValueOfResource(resourceType);
                resourceSlider.value = player.GetCurrentResource(resourceType);
                textElement.text = resourceSlider.value.ToString() + "/" + resourceSlider.maxValue.ToString();
            }
            else if (value.signal.Equals(GameSignal.RESOURCE_MODIFY) && (value as CombatResourceSignalData).resourceType.Equals(resourceType))
            {
                resourceSlider.value = player.GetCurrentResource(resourceType);
                textElement.text = resourceSlider.value.ToString() + "/" + resourceSlider.maxValue.ToString();
            }
            else if (value.signal.Equals(GameSignal.MAX_RESOURCE_MODIFY) && (value as ResourceSignalData).resourceType.Equals(resourceType))
            {
                resourceSlider.maxValue = player.GetMaxValueOfResource(resourceType);
                textElement.text = resourceSlider.value.ToString() + "/" + resourceSlider.maxValue.ToString();
            }
        }
    }

}
