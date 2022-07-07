using Abilities.Passive;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace OdinEditor
{
    public class PassiveAbilityDrawer<TPassive> : OdinValueDrawer<TPassive>
       where TPassive : PassiveAbility
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

            PassiveAbility ability = this.ValueEntry.SmartValue;
            Texture texture = null;

            if (ability)
            {
                texture = GUIHelper.GetAssetThumbnail(ability.GetSprite(), typeof(TPassive), true);
                GUI.Label(rect.AddXMin(50).AlignMiddle(16), EditorGUI.showMixedValue ? "-" : ability.GetName());
            }

            this.ValueEntry.WeakSmartValue = SirenixEditorFields.UnityPreviewObjectField(rect.AlignLeft(45), ability, texture, this.ValueEntry.BaseValueType);
        }
    }

}