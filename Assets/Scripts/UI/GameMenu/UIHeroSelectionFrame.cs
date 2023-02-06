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
        [SerializeField] TextMeshProUGUI className;
        [SerializeField] List<GameObject> rows;

        public void SetVariables(List<Item> items, List<BaseBuff> traits, PlayerClass heroClass, Sprite classSprite, GameObject prefab)
        {
            heroSprite.sprite = classSprite;
            SimpleTooltipStyle tooltipStyle = UIManager.manager.GetTooltipStyle();
            className.text = heroClass.GetName();
            int count = 0;
            foreach (var item in items)
            {
                rows[count].SetActive(true);
                rows[count].transform.Find("Icon").GetComponent<Image>().sprite = item.GetSprite();
                rows[count].GetComponentInChildren<TextMeshProUGUI>().text = UtilsClass.instance.ConvertTextWithStyles(item.GetName(), tooltipStyle);
                rows[count].GetComponent<SimpleTooltip>().simpleTooltipStyle = tooltipStyle;
                rows[count].GetComponent<SimpleTooltip>().enabled = true;
                item.SetTooltipText(rows[count].GetComponent<SimpleTooltip>());
                count++;
            }
            foreach (var item in traits)
            {
                rows[count].SetActive(true);
                rows[count].transform.Find("Icon").GetComponent<Image>().sprite = item.GetIcon();
                rows[count].GetComponentInChildren<TextMeshProUGUI>().text = UtilsClass.instance.ConvertTextWithStyles(item.GetName(), tooltipStyle);
                rows[count].GetComponent<SimpleTooltip>().enabled = true;
                rows[count].GetComponent<SimpleTooltip>().simpleTooltipStyle = tooltipStyle;
                item.SetTooltipText(rows[count].GetComponent<SimpleTooltip>());
                count++;
            }

            GetComponentInChildren<Button>().onClick.AddListener(() => HeroGenerator.Instance.StartGame(heroClass, items, traits, prefab));
        }
    }

}
