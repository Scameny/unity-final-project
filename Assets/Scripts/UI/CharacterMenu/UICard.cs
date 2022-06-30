using CardSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class UICard : MonoBehaviour
    {
        [SerializeField] Image cardImage;
        [SerializeField] TextMeshProUGUI cardName, cardDescription;
        [SerializeField] GameObject cost;
        
        
        public void InitializeCard(Usable cardUse)
        {
            cardImage.sprite = cardUse.GetSprite();
            cardName.text = (cardUse).GetName();
            cardDescription.text = UtilsClass.instance.ConvertTextWithStyles((cardUse).GetDescription(), UIManager.manager.tooltipStyle);
            if (cardUse.GetResourceCosts().Count > 0)
            {
                //TODO add support to more than one resource cost
                cost.GetComponentInChildren<TextMeshProUGUI>().text = cardUse.GetResourceCosts()[0].amount.ToString();
                // TODO add change of resource background depending on resource type
            }
        }
    }
}

