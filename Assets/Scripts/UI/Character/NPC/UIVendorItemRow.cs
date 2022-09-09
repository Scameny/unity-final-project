using Character.Character;
using Interaction;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.Character.NPC
{
    public class UIVendorItemRow : MonoBehaviour
    {
        [SerializeField] Image icon;
        [SerializeField] TextMeshProUGUI priceText, itemName;
        ItemToSell item;
        Hero player;

        public void InitializeRow(ItemToSell item, Hero player)
        {
            this.player = player;
            this.item = item;
            icon.sprite = item.item.GetSprite();
            itemName.text = UtilsClass.instance.ConvertTextWithStyles(item.item.GetName(), UIManager.manager.GetTooltipStyle());
            priceText.text = item.price + " Coins";
            item.item.SetTooltipText(GetComponent<SimpleTooltip>());
        }

        public void OnClick()
        {
            if (player.UseCoins(item.price))
            {
                player.AddItem(item.item);
                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("Not enough money");
            }
        }


    }
}
