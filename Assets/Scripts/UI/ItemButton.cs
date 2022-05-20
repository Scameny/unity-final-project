using Combat;
using Character.Character;
using Items;
using UnityEngine;


namespace UI
{
    public class ItemButton : MonoBehaviour
    {
        public Item item;

        public void UseItem(HeroCombat player)
        {
            player.UseItem((UsableItem)item);
            transform.parent.gameObject.SetActive(false);
            if (!((Hero)player.character).inventory.ExistItem(item))
            {
                Destroy(gameObject);
            }
        }
    }

}
