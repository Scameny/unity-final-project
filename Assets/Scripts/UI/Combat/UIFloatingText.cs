using Character.Buff;
using Character.Stats;
using DG.Tweening;
using GameManagement;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Combat
{
    public class UIFloatingText : MonoBehaviour
    {
        [SerializeField] float duration;
        [SerializeField] float timeBetweenText;
        [SerializeField] List<ResourceTypeColor> resourceTypeColors = new List<ResourceTypeColor>();
        [SerializeField] List<BuffSignalColor> buffSignalColors = new List<BuffSignalColor>();

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
            GetComponentInChildren<Image>().enabled = false;
            GetComponentInChildren<TextMeshProUGUI>().color = GetColor(type);
            GetComponentInChildren<TextMeshProUGUI>().text = (value > 0 ? "+" : "") + value.ToString() + " " + type.ToString();
            StartMovement();
        }

        public void SetValues(BaseBuff buff, GameSignal gameSignal)
        {
            GetComponentInChildren<TextMeshProUGUI>().color = GetColor(gameSignal);
            GetComponentInChildren<TextMeshProUGUI>().text = buff.GetName();
            GetComponentInChildren<Image>().enabled = true;
            GetComponentInChildren<Image>().sprite = buff.GetIcon();
            StartMovement();
        }

        private Color GetColor(ResourceType type)
        {
            foreach (var item in resourceTypeColors)
            {
                if (item.incomingType == type)
                    return item.color;
            }
            Debug.LogWarning("Doesn't exist color for the incoming type " + type.ToString() + ". Using default color (grey)");
            return Color.grey;
        }

        private Color GetColor(GameSignal type)
        {
            foreach (var item in buffSignalColors)
            {
                if (item.signal == type)
                    return item.color;
            }
            Debug.LogWarning("Doesn't exist color for the incoming signal " + type.ToString() + ". Using default color (grey)");
            return Color.grey;
        }

        public float GetTimeBetweenText()
        {
            return timeBetweenText;
        }
    }


    [Serializable]
    public struct ResourceTypeColor
    {
        public ResourceType incomingType;
        public Color color;

    }

    [Serializable]
    public struct BuffSignalColor
    {
        public GameSignal signal;
        public Color color;

    }
}
