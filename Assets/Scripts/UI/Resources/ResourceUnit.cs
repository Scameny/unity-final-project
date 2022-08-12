using Character.Character;
using Character.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ResourceUnit : MonoBehaviour
    {
        public ResourceType resourceType;

        Slider resourceSlider;
        TextMeshProUGUI textElement;
        DefaultCharacter player;

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
