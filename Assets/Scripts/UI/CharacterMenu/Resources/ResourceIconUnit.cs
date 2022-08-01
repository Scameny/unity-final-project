using Character.Character;
using Character.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ResourceIconUnit : MonoBehaviour
    {
        public ResourceType resourceType;

        TextMeshProUGUI textElement;
        Image icon;
        DefaultCharacter player;


        private void Awake()
        {
            textElement = GetComponentInChildren<TextMeshProUGUI>();
            icon = GetComponentInChildren<Image>();
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