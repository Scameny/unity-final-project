using Character.Character;
using Items;
using TMPro;
using UnityEngine;


namespace UI
{
    public class UIInventory : MonoBehaviour
    {
        Hero player;
        UIInventorySlot[] slots;
        [SerializeField] TextMeshProUGUI currentCoinsText;

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Hero>();
            slots = GetComponentsInChildren<UIInventorySlot>();
            for (int i = 0; i < player.GetAllStoredItems().GetLength(0); i++)
            {
                slots[i].position = i;
            }
        }

        private void OnEnable()
        {
            if (player==null)
                player = GameObject.FindGameObjectWithTag("Player").GetComponent<Hero>();
            currentCoinsText.text = player.GetCoins().ToString();
            Item[] itemsInInventory = player.GetAllStoredItems();
            for (int i = 0; i < itemsInInventory.Length; i++)
            {
                if (itemsInInventory[i] != null)
                    slots[i].GetDragableItem().SetItem(itemsInInventory[i]);
            }
        }
    }

}