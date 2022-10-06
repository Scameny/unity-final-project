using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using Abilities.ability;
using Character.Classes;
using Sirenix.Utilities;
using Items;
using Abilities.Passive;

namespace OdinEditor
{ 
    public class DataEditor : OdinMenuEditorWindow
    {
        [MenuItem("Tools/Data editor")]
        private static void OpenWindow()
        {
            GetWindow<DataEditor>().Show();
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();
            tree.Config.DrawSearchToolbar = true;

            tree.Selection.SupportsMultiSelect = false;
            tree.AddAllAssetsAtPath("Player Classes", "Assets/Game/Core/Character/Classes", typeof(PlayerClass), true);
            tree.AddAllAssetsAtPath("Enemy Classes", "Assets/Game/Core/Character/EnemiesClasses", typeof(NPCClass), true);
            tree.AddAllAssetsAtPath("Abilities", "Assets/Game/Core/Character/Abilities", typeof(Ability), true)
                .ForEach(this.AddDragHandles);
            tree.AddAllAssetsAtPath("Items", "Assets/Game/Core/Items", typeof(Item), true);
            tree.AddAllAssetsAtPath("PassiveAbilities", "Assets/Game/Core/Character/Abilities", typeof(PassiveAbility), true)
                .ForEach(this.AddDragHandles);

            // Add icons to characters and items.
            tree.EnumerateTree().AddIcons<Ability>(x => x.GetSprite());
            tree.EnumerateTree().AddIcons<PassiveAbility>(x => x.GetSprite());
            tree.EnumerateTree().AddIcons<Item>(x => x.GetSprite());
            tree.EnumerateTree().AddIcons<DefaultAsset>(x => EditorIcons.Folder.Raw);
            return tree;
        }

        private void AddDragHandles(OdinMenuItem menuItem)
        {
            menuItem.OnDrawItem += x => DragAndDropUtilities.DragZone(menuItem.Rect, menuItem.Value, false, false);
        }



        protected override void OnBeginDrawEditors()
        {
            OdinMenuTreeSelection selected = null;
            if (this.MenuTree != null)
                selected = this.MenuTree.Selection;

            SirenixEditorGUI.BeginHorizontalToolbar();
            {
                GUILayout.FlexibleSpace();

                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create ability")))
                {
                    ScriptableObjectCreator.ShowDialog<Ability>("Assets/Game/Core/Abilities/", obj =>
                    {
                        base.TrySelectMenuItemWithObject(obj); // Selects the newly created item in the editor
                    });
                }

                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create passive ability")))
                {
                    ScriptableObjectCreator.ShowDialog<PassiveAbility>("Assets/Game/Core/Abilities/", obj =>
                    {
                        base.TrySelectMenuItemWithObject(obj); // Selects the newly created item in the editor
                    });
                }

                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create character class")))
                {
                    ScriptableObjectCreator.ShowDialog<CharacterClass>("Assets/Game/Core/Classes/", obj =>
                    {
                        base.TrySelectMenuItemWithObject(obj); // Selects the newly created item in the editor
                    });
                }

                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create npc class")))
                {
                    ScriptableObjectCreator.ShowDialog<Ability>("Assets/Game/Core/EnemiesClasses/", obj =>
                    {
                        base.TrySelectMenuItemWithObject(obj); // Selects the newly created item in the editor
                    });
                }

                if (SirenixEditorGUI.ToolbarButton("Delete Current") && selected != null)
                {
                    ScriptableObject asset = selected.SelectedValue as ScriptableObject;
                    string path = AssetDatabase.GetAssetPath(asset);
                    AssetDatabase.DeleteAsset(path);
                    AssetDatabase.SaveAssets();
                }

            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }
    }
}