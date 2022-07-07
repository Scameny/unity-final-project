using Items;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor.Drawers;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEngine;

namespace OdinEditor
{
    internal sealed class InventoryDrawer<TArray> : TwoDimensionalArrayDrawer<TArray, Item>
           where TArray : System.Collections.IList
    {
        protected override TableMatrixAttribute GetDefaultTableMatrixAttributeSettings()
        {
            return new TableMatrixAttribute()
            {
                SquareCells = true,
                HideColumnIndices = true,
                HideRowIndices = true,
                ResizableColumns = false
            };
        }

        protected override Item DrawElement(Rect rect, Item value)
        {
            var id = DragAndDropUtilities.GetDragAndDropId(rect);
            DragAndDropUtilities.DrawDropZone(rect, value ? value.GetSprite(): null, null, id); // Draws the drop-zone using the items icon.

            value = DragAndDropUtilities.DropZone(rect, value);                                     // Drop zone for ItemSlot structs.
            value = DragAndDropUtilities.DropZone<Item>(rect, value);                     // Drop zone for Item types.
            value = DragAndDropUtilities.DragZone(rect, value, true, true);                         // Enables dragging of the ItemSlot

            return value;
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            base.DrawPropertyLayout(label);

            // Draws a drop-zone where we can destroy items.
            var rect = GUILayoutUtility.GetRect(0, 20).Padding(2);
            var id = DragAndDropUtilities.GetDragAndDropId(rect);
            DragAndDropUtilities.DrawDropZone(rect, null as UnityEngine.Object, null, id);
            DragAndDropUtilities.DropZone<Item>(rect, null, false, id);
        }
    }
}