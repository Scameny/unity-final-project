using Character.Character;
using Character.Stats;
using GameManagement;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.CharacterResources
{
    public class ResourceIconUnit : MonoBehaviour, IObserver<SignalData>
    {
        public ResourceType resourceType;

        TextMeshProUGUI textElement;
        Image icon;
        DefaultCharacter player;
        IDisposable disposable;

        private void Awake()
        {
            textElement = GetComponentInChildren<TextMeshProUGUI>();
            icon = GetComponentInChildren<Image>();
        }

      
        private void Start()
        {
            disposable = UIManager.manager.Subscribe(this);
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
            if (value.signal.Equals(GameSignal.START_COMBAT) || value.signal.Equals(GameSignal.START_GAME))
            {
                UpdateResourceUnit();
            }
            else if (value.signal.Equals(GameSignal.RESOURCE_MODIFY) && (value as CombatResourceSignalData).user.Equals(player.gameObject) &&
                (value as CombatResourceSignalData).resourceType.Equals(resourceType))
            {
                UpdateResourceUnit();
            }
            else if (value.signal.Equals(GameSignal.OUT_OF_COMBAT_CURRENT_RESOURCE_MODIFY) && (value as ResourceSignalData).user.Equals(player.gameObject) &&
                (value as ResourceSignalData).resourceType.Equals(resourceType))
            {
                UpdateResourceUnit();
            }
        }
    }
}