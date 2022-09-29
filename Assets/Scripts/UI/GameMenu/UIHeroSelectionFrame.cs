using Character.Buff;
using Character.Classes;
using GameManagement.HeroGenerator;
using Items;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.GameMenu
{
    public class UIHeroSelectionFrame : MonoBehaviour
    {
        [SerializeField] Image heroSprite;
        [SerializeField] TextMeshProUGUI className, itemsTittle, traitsTittle;
        [SerializeField] List<GameObject> itemsRow;
        [SerializeField] List<GameObject> traitsRow;

        public void SetVariables(List<Item> items, List<BaseBuff> traits, PlayerClass heroClass, Sprite classSprite, GameObject prefab)
        {
            heroSprite.sprite = classSprite;
            SimpleTooltipStyle tooltipStyle = UIManager.manager.GetTooltipStyle();
            className.text = heroClass.GetName();
            int count = 0;
            itemsTittle.gameObject.SetActive(items.Count > 0);
            foreach (var item in items)
            {
                itemsRow[count].SetActive(true);
                itemsRow[count].transform.Find("Icon").GetComponent<Image>().sprite = item.GetSprite();
                itemsRow[count].GetComponentInChildren<TextMeshProUGUI>().text = UtilsClass.instance.ConvertTextWithStyles(item.GetName(), tooltipStyle);
                itemsRow[count].GetComponent<SimpleTooltip>().simpleTooltipStyle = tooltipStyle;
                itemsRow[count].GetComponent<SimpleTooltip>().enabled = true;
                item.SetTooltipText(itemsRow[count].GetComponent<SimpleTooltip>());
                count++;
            }
            count = 0;
            traitsTittle.gameObject.SetActive(traits.Count > 0);
            foreach (var item in traits)
            {
                traitsRow[count].SetActive(true);
                traitsRow[count].transform.Find("Icon").GetComponent<Image>().sprite = item.GetIcon();
                traitsRow[count].GetComponentInChildren<TextMeshProUGUI>().text = UtilsClass.instance.ConvertTextWithStyles(item.GetName(), tooltipStyle);
                traitsRow[count].GetComponent<SimpleTooltip>().enabled = true;
                traitsRow[count].GetComponent<SimpleTooltip>().simpleTooltipStyle = tooltipStyle;
                item.SetTooltipText(traitsRow[count].GetComponent<SimpleTooltip>());
                count++;
            }

            GetComponentInChildren<Button>().onClick.AddListener(() => HeroGenerator.Instance.StartGame(heroClass, items, traits, prefab));
        }
    }

}
