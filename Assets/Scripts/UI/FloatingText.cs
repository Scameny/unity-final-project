using Character.Stats;
using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI
{
    public class FloatingText : MonoBehaviour
    {
        public float timeBetweenText;
        [SerializeField] float duration;
        [SerializeField] List<IncomingTypeColor> incomingTypeColors = new List<IncomingTypeColor>();

        // Start is called before the first frame update
        void StartMovement()
        {
            transform.DOMove(transform.position + Vector3.down, duration).OnComplete(() =>
                {
                    Destroy(gameObject);
                }
                );
        }

        public void SetValues(ResourceType type, int value)
        {
            GetComponent<TextMeshProUGUI>().color = GetColor(type);
            GetComponent<TextMeshProUGUI>().text = (value > 0 ? "+" : "") + value.ToString() + " " + type.ToString();
            StartMovement();
        }

        private Color GetColor(ResourceType type)
        {
            foreach (var item in incomingTypeColors)
            {
                if (item.incomingType == type)
                    return item.color;
            }
            Debug.LogError("Doesn't exist color for the incoming type " + type.ToString());
            throw new MissingRequiredParameterException(type.ToString(), name);
        }
    }


    [Serializable]
    public struct IncomingTypeColor
    {
        public ResourceType incomingType;
        public Color color;

    }
}
