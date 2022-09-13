using Character.Character;
using GameManagement;
using Items;
using System;
using TMPro;
using UI.UIElements;
using UnityEngine;


namespace UI.Items
{
    public class UIInventory : MonoBehaviour, IObserver<SignalData>
    {
        Hero player;
        UIInventorySlot[] slots;
        [SerializeField] TextMeshProUGUI currentCoinsText;
        IDisposable disposable;

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Hero>();
            slots = GetComponentsInChildren<UIInventorySlot>();
            for (int i = 0; i < player.GetAllStoredItems().GetLength(0); i++)
            {
                slots[i].position = i;
            }
            disposable = UIManager.manager.Subscribe(this);
        }

        public void OnCompleted()
        {
            disposable.Dispose();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(SignalData value)
        {
            if (value.signal.Equals(GameSignal.OPEN_CHARACTER_MENU))
            {
                currentCoinsText.text = player.GetCoins().ToString();
                Item[] itemsInInventory = player.GetAllStoredItems();
                for (int i = 0; i < itemsInInventory.Length; i++)
                {
                    if (itemsInInventory[i] != null)
                        slots[i].GetDragableItem().SetItem(itemsInInventory[i]);
                }
                gameObject.SetActive(true);
            } 
            else if (value.signal.Equals(GameSignal.CLOSE_CHARACTER_MENU) || value.signal.Equals(GameSignal.START_GAME))
            {
                gameObject.SetActive(false);
            }

        }
    }

}