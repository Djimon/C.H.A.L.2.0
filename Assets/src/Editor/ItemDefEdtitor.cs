#if UNITY_EDITOR
using CHAL.Data;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemDef))]
public class ItemDefEditor : Editor
{
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

        // Typ-spezifische Felder
        if (item.itemId.StartsWith("remains:"))
        {
            item.remainData.remainType = EditorGUILayout.TextField("remain Type", item.remainData?.remainType);
        }
        else if (item.itemId.StartsWith("rune:"))
        {
            item.runeData.effectType = EditorGUILayout.TextField("Effect Type", item.runeData?.effectType);
            item.runeData.hexColor = EditorGUILayout.TextField("Rune Color (HEX)",item.runeData.hexColor);
        }
        else if (item.itemId.StartsWith("part:"))
        {
            item.partData.dnaType = EditorGUILayout.TextField("DNA Type", item.partData?.dnaType);
        }
        else if (item.itemId.StartsWith("module:"))
        {
            item.moduleData.modulePower = EditorGUILayout.FloatField("Base Power", item.moduleData?.modulePower ?? 0.0f);
            item.moduleData.effect = EditorGUILayout.TextField("Effect", item.moduleData?.effect);
        }

        // Änderungen speichern
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
#endif