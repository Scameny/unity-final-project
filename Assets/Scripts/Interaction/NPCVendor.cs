using DialogueEditor;
using GameManagement;
using Items;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UI;
using UnityEngine;
using Utils;

namespace Interaction
{
    public class NPCVendor : NPCInteractable
    {
        [Required]
        [SerializeField] NPCConversation defaultVendorInteraction;
        [SerializeField] int maxNumOfItems = 2;
        [SerializeField] int minNumOfItems = 1;
        [SerializeField] List<PoolObject<ItemToSell>> vendorPool;

        List<ItemToSell> itemsToBuy = new List<ItemToSell>();

        public void Start()
        {
            SetVendorItems();
        }

        public override void StartConversation()
        {
            base.StartConversation();
            ConversationManager.Instance.StartConversation(defaultVendorInteraction);
        }

        public override void Interact()
        {
            UIManager.manager.SendData(new UINpcSignalData(GameSignal.ASSIGN_NPC_UI_ELEMENT, UIElement.VENDOR_FRAME, true, this));
        }

        public override void OnEndInteract()
        {
            base.OnEndInteract();
            UIManager.manager.SendData(new UISignalData(GameSignal.ENABLE_UI_ELEMENT, UIElement.VENDOR_FRAME, false));
        }

        private void SetVendorItems()
        {
            int numberOfItems = Random.Range(minNumOfItems, maxNumOfItems + 1);
            int totalWeigth = 0;
            foreach (var item in vendorPool)
            {
                totalWeigth += item.weigth;
            }
            for (int i = 0; i < numberOfItems; i++)
            {
                int randomNum = Random.Range(0, totalWeigth);
                int aux = 0;
                int indexItemSelected = 0;

                for (int j = 0; j < vendorPool.Count; j++)
                {
                    if (randomNum < vendorPool[j].weigth + aux)
                    {
                        indexItemSelected = j;
                        break;
                    }
                    else
                    {
                        aux += vendorPool[j].weigth;
                    }
                }
                itemsToBuy.Add(vendorPool[indexItemSelected].gameObject);
                vendorPool.RemoveAt(indexItemSelected);
            }
        }

        public List<ItemToSell> GetItemsToBuy()
        {
            return itemsToBuy;
        }

        public void OnSellItem(ItemToSell item)
        {
            itemsToBuy.Remove(item);
        }


        private void OnValidate()
        {
            if (vendorPool.Count == 0)
                Debug.LogError("Vendor pool is empty for prefab " + gameObject.name);
            if (vendorPool.Count < maxNumOfItems)
                Debug.LogError("Vendor pool has less items than the max number of items to sell in gameObject" + gameObject.name);
        }
    }

    [System.Serializable]
    [InlineProperty]
    [HideLabel, FoldoutGroup("Item to sell", Expanded = true)]
    public class ItemToSell
    {
        [HorizontalGroup("Base")]

        [LabelWidth(30), Space(10)]
        [VerticalGroup("Base/Left")]
        public int price;
        [HideLabel]
        [VerticalGroup("Base/Right")]
        public Item item;
    }
}
