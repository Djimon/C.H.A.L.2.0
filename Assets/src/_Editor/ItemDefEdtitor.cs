#if UNITY_EDITOR
using CHAL.Data;
using UnityEditor;
using UnityEngine;


    [CustomEditor(typeof(ItemDef))]
/// <summary>
/// Provides a custom editor for the ItemDef object in the Unity Inspector.
/// This class allows users to edit item properties visually.
/// </summary>
    public class ItemDefEditor : Editor
    {
/// <summary>
/// Draws the custom inspector GUI for the ItemDef object.
/// This method allows editing of item properties in the Unity Inspector.
/// </summary>
    public override void OnInspectorGUI()
    {
        var item = (ItemDef)target;

        // Basisfelder immer anzeigen
        item.itemId = EditorGUILayout.TextField("Item ID", item.itemId);
        //item.displayName = EditorGUILayout.TextField("Display Name", item.displayName);
        item.icon = (Sprite)EditorGUILayout.ObjectField("Icon", item.icon, typeof(Sprite), false);
        item.rarity = (Rarity)EditorGUILayout.EnumPopup("Rarity", item.rarity);
        item.lootValue = EditorGUILayout.IntField("Loot Value", item.lootValue);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Type Specific", EditorStyles.boldLabel);

        // Typ-spezifische Felder
        if (item.itemId.StartsWith("remains:"))
        {
            Ensure(ref item.remainData);
            item.remainData.remainType = EditorGUILayout.TextField("Remain Type", item.remainData.remainType);
        }
        else if (item.itemId.StartsWith("rune:"))
        {
            Ensure(ref item.runeData);
            item.runeData.effectType = EditorGUILayout.TextField("Effect Type", item.runeData.effectType);
            item.runeData.runeColortType = (RuneColorType)EditorGUILayout.EnumPopup("Rune Color", item.runeData.runeColortType);
        }
        else if (item.itemId.StartsWith("part:"))
        {
            Ensure(ref item.partData);
            item.partData.dnaType = EditorGUILayout.TextField("DNA Type", item.partData.dnaType);
            // (moduleFuel lässt du aktuell im Editor weg – kannst du später ergänzen)
        }
        else if (item.itemId.StartsWith("module:"))
        {
            Ensure(ref item.moduleData);

            // SkillDef Reference (ObjectField)
            EditorGUI.BeginChangeCheck();
            var newSkillDef = (SkillModuleDef)EditorGUILayout.ObjectField(
                "Skill Def",
                item.moduleData.skillDef,
                typeof(SkillModuleDef),
                false);

            // SkillId anzeigen (read-only, weil OnValidate das setzt)
            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUILayout.TextField("Referenced SkillId (auto)", item.moduleData.skillId);
            }

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(item, "Edit Module Data");
                item.moduleData.skillDef = newSkillDef;

                // Optional: sofort skillId syncen (falls du nicht auf OnValidate warten willst)
                if (item.moduleData.skillDef != null)
                    item.moduleData.skillId = item.moduleData.skillDef.SkillId;

                EditorUtility.SetDirty(item);
            }

        }
        else if (item.itemId.StartsWith("gear:"))
        {
            Ensure(ref item.gearData);
            item.gearData.slotType = (GearType)EditorGUILayout.EnumPopup("Slot Type", item.gearData.slotType);
            DrawStringArray(ref item.gearData.tags, "Tag");
            item.gearData.armorClass = (ArmorClass)EditorGUILayout.EnumPopup("Armor Class", item.gearData.armorClass);
        }
        else if (item.itemId.StartsWith("core:"))
        {
            Ensure(ref item.coreData);
            //item.coreData.defualtDmgType = (DamageType)EditorGUILayout.EnumPopup("Damage Type", item.coreData.defualtDmgType);
            item.coreData.coreType = (CoreType) EditorGUILayout.EnumPopup("coreType", item.coreData.coreType);
        }
        else
        {
            EditorGUILayout.HelpBox("Unbekannter Item-Prefix. Unterstützt: remains:, rune:, part:, module:, gear:", MessageType.Info);
        }

        // Änderungen speichern
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }

        // Hilfs-Funktionen
        static void Ensure<T>(ref T field) where T : class, new()
        {
            if (field == null) field = new T();
        }

        void DrawStringArray(ref string[] arr, string label)
        {
            int size = Mathf.Max(0, EditorGUILayout.IntField($"{label} Count", arr?.Length ?? 0));
            if (arr == null || arr.Length != size)
            {
                var newArr = new string[size];
                if (arr != null)
                {
                    for (int i = 0; i < Mathf.Min(arr.Length, size); i++) newArr[i] = arr[i];
                }
                arr = newArr;
            }
            EditorGUI.indentLevel++;
            for (int i = 0; i < size; i++)
                arr[i] = EditorGUILayout.TextField($"{label} [{i}]", arr[i]);
            EditorGUI.indentLevel--;
        }
    }
}
#endif
