using Items;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace OdinEditor
{
    public class ItemDrawer<TGearItem> : OdinValueDrawer<TGearItem>
       where TGearItem : Item
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            var rect = EditorGUILayout.GetControlRect(label != null, 45);

            if (label != null)
            {
                rect.xMin = EditorGUI.PrefixLabel(rect.AlignCenterY(15), label).xMin;
            }
            else
            {
                rect = EditorGUI.IndentedRect(rect);
            }

            Item item = this.ValueEntry.SmartValue;
            Texture texture = null;

            if (item)
            {
                texture = GUIHelper.GetAssetThumbnail(item.GetSprite(), typeof(TGearItem), true);
                GUI.Label(rect.AddXMin(50).AlignMiddle(16), EditorGUI.showMixedValue ? "-" : item.name);
            }

            this.ValueEntry.WeakSmartValue = SirenixEditorFields.UnityPreviewObjectField(rect.AlignLeft(45), item, texture, this.ValueEntry.BaseValueType);
        }
    }

}