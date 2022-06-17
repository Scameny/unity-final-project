using CardSystem;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class UsableDrawer<TItem> : OdinValueDrawer<TItem>
       where TItem : Usable
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

            Usable ability = this.ValueEntry.SmartValue;
            Texture texture = null;

            if (ability)
            {
                texture = GUIHelper.GetAssetThumbnail(ability.GetSprite(), typeof(TItem), true);
                GUI.Label(rect.AddXMin(50).AlignMiddle(16), EditorGUI.showMixedValue ? "-" : ability.name);
            }

            this.ValueEntry.WeakSmartValue = SirenixEditorFields.UnityPreviewObjectField(rect.AlignLeft(45), ability, texture, this.ValueEntry.BaseValueType);
        }
    }

}