using Sirenix.OdinInspector;
using UnityEngine;

namespace Items
{
    public abstract class Item : ScriptableObject
    {
        protected const string GENERAL_SETTINGS_GROUP = "Base/Right/Top/General Settings";
        protected const string GENERAL_SETTINGS_POSITION = "Base/Right/Top";


        [HorizontalGroup("Base", Width = 150)]

        [HideLabel, PreviewField(150)]
        [VerticalGroup("Base/Left")]
        [SerializeField] Sprite sprite;

        [VerticalGroup("Base/Right")]
        [HorizontalGroup(GENERAL_SETTINGS_POSITION)]
        [BoxGroup(GENERAL_SETTINGS_GROUP)]
        [SerializeField] string Name;

        [BoxGroup(GENERAL_SETTINGS_GROUP)]
        [SerializeField] ItemRarity itemRarity;

        [HorizontalGroup("Base/Right/Bottom")]
        [TextArea(4, 14)]
        [SerializeField] string description;

        

        public abstract ItemType GetItemType();

        public Sprite GetSprite()
        {
            return sprite;
        }

        public ItemRarity GetItemRarity()
        {
            return itemRarity;
        }

        public string GetDescription()
        {
            return description;
        }

        public string GetName()
        {
            return Name;
        }
    }


    public enum ItemType
    {
        Equipable,
        KeyItem
    }

    public enum ItemRarity
    {
        Common,
        Rare,
        Epic,
        Legendary
    }
}
