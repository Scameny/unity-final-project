using Character.Character;
using Interaction;
using UnityEngine;

namespace UI
{
    public class UIVendorFrame : MonoBehaviour
    {
        [SerializeField] GameObject itemRow;
        [SerializeField] Transform content;
        Hero player;
        NPCVendor npc;

        private void OnEnable()
        {
            if (npc == null)
                return;
            else
            {
                player = GameObject.FindGameObjectWithTag("Player").GetComponent<Hero>();
                InitializeItemsRow();
            }
        }

        private void OnDisable()
        {
            npc = null;
        }

        public void SetVendorNpc(NPCVendor npc)
        {
            this.npc = npc;
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
    }
}
