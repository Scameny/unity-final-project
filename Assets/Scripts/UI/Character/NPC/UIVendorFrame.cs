using Character.Character;
using GameManagement;
using Interaction;
using System;
using UnityEngine;

namespace UI.Character.NPC
{
    public class UIVendorFrame : MonoBehaviour, IObserver<SignalData>
    {
        [SerializeField] GameObject itemRow;
        [SerializeField] Transform content;


        Hero player;
        NPCVendor npc;
        IDisposable disposable;


        private void Start()
        {
            disposable = UIManager.manager.Subscribe(this);
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Hero>();
            gameObject.SetActive(false);
        }


        private void OnDisable()
        {
            npc = null;
            RemoveItemsRow();
        }


        private void InitializeItemsRow()
        {
            foreach (var item in npc.GetItemsToBuy())
            {
                GameObject newItemRow = Instantiate(itemRow, content);
                newItemRow.GetComponent<UIVendorItemRow>().InitializeRow(item, player);
            }
        }

        private void RemoveItemsRow()
        {
            for (int i = 0; i < content.childCount; i++)
            {
                Destroy(content.GetChild(i).gameObject);
            }
        }

        public void CloseMenu()
        {
            RemoveItemsRow();
            npc.OnEndInteract();
        }

        public void OnCompleted()
        {
            disposable.Dispose();
        }

        public void OnError(Exception error)
        {
            Debug.LogError("Error on vendor frame: " + error.Message);
        }

        public void OnNext(SignalData value)
        {
            if (value.signal.Equals(GameSignal.ASSIGN_NPC_UI_ELEMENT) && (value as UINpcSignalData).element.Equals(UIElement.VENDOR_FRAME))
            {
                npc = (value as UINpcSignalData).npc as NPCVendor;
                InitializeItemsRow();
                gameObject.SetActive((value as UINpcSignalData).enable);
            }
            else if (value.signal.Equals(GameSignal.ENABLE_UI_ELEMENT) && (value as UISignalData).element.Equals(UIElement.VENDOR_FRAME))
            {
                gameObject.SetActive((value as UISignalData).enable);
                if ((value as UISignalData).enable && npc == null)
                    OnError(new Exception("Missing npc"));
            }
            else if (value.signal.Equals(GameSignal.START_GAME))
            {
                gameObject.SetActive(false);
            }
        }
    }
}
