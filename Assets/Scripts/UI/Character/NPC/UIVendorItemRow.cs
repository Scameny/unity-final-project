using Character.Character;
using GameManagement;
using Interaction;
using System.Collections.Generic;
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
        NPCVendor vendor;


        public void InitializeRow(ItemToSell item, Hero player, NPCVendor vendor)
        {
            this.player = player;
            this.vendor = vendor;
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
                vendor.OnSellItem(item);
                Destroy(gameObject);
            }
            else
            {
                UIManager.manager.SendData(new ErrorSignalData(GameSignal.NOT_ENOUGH_MONEY, new List<string>()));
                Debug.LogError("Not enough money");
            }
        }


    }
}
