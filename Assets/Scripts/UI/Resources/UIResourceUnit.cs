using Character.Character;
using Character.Stats;
using GameManagement;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.CharacterResources
{
    public class UIResourceUnit : MonoBehaviour, IObserver<SignalData>
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
            disposable = UIManager.manager.Subscribe(this);
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<DefaultCharacter>();
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
                if (player.HasResource(resourceType))
                {
                    gameObject.SetActive(true);
                    resourceSlider.maxValue = player.GetMaxValueOfResource(resourceType);
                    resourceSlider.value = player.GetCurrentResource(resourceType);
                    textElement.text = resourceSlider.value.ToString() + "/" + resourceSlider.maxValue.ToString();
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
            else if (value.signal.Equals(GameSignal.RESOURCE_MODIFY) && (value as CombatResourceSignalData).user.Equals(player.gameObject) &&
                (value as CombatResourceSignalData).resourceType.Equals(resourceType))
            {
                resourceSlider.value = player.GetCurrentResource(resourceType);
                textElement.text = resourceSlider.value.ToString() + "/" + resourceSlider.maxValue.ToString();
            }
            else if (value.signal.Equals(GameSignal.OUT_OF_COMBAT_CURRENT_RESOURCE_MODIFY) && (value as ResourceSignalData).user.Equals(player.gameObject) &&
                (value as ResourceSignalData).resourceType.Equals(resourceType))
            {
                resourceSlider.value = player.GetCurrentResource(resourceType);
                textElement.text = resourceSlider.value.ToString() + "/" + resourceSlider.maxValue.ToString();
            }
            else if (value.signal.Equals(GameSignal.MAX_RESOURCE_MODIFY) && (value as ResourceSignalData).resourceType.Equals(resourceType))
            {
                resourceSlider.maxValue = player.GetMaxValueOfResource(resourceType);
                textElement.text = resourceSlider.value.ToString() + "/" + resourceSlider.maxValue.ToString();
            }
            else if (value.signal.Equals(GameSignal.ADD_RESOURCE) || value.signal.Equals(GameSignal.REMOVE_RESOURCE))
            {
                gameObject.SetActive(player.HasResource(resourceType));
            }
        }
    }

}
