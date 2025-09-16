using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DebugConfig", menuName = "Config/DebugConfig")]
public class DebugConfig : ScriptableObject
{
    //Create asset of Type "DebugConfig" and save it under "Resources/config/DebugConfig.asset"

    public DebugManager.EDebugLevel level = DebugManager.EDebugLevel.Debug;
    public bool productiveMode = false;

    // Wenn true: unbekannte Tags, die erstmalig geloggt werden,
    // automatisch ins Asset übernehmen (mit Standardfarbe weiß, active=true).
    public bool autoAddUnknownTagsToAsset = false;

    [System.Serializable]
    public class TagEntry
    {
        public string name;
        public bool active = true;
        public Color color = Color.white;
    }

    public List<TagEntry> tags = new();

#if UNITY_EDITOR
    [HideInInspector][SerializeField] private bool _seeded = false;

    // Beim ersten Anlegen / Editieren Defaults ins Asset schreiben
    private void OnValidate()
    {
        if (!_seeded || tags == null || tags.Count == 0)
        {
            EnsureTag("System", Color.yellow, true);
            EnsureTag("Info", Color.green, true);
            EnsureTag("Debug", Color.white, true);
            EnsureTag("Warning", new Color(1f, 0.64f, 0f), true); // orange
            EnsureTag("Error", Color.red, true);

            _seeded = true;
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssets();
        }
    }

    private void EnsureTag(string name, Color color, bool active)
    {
        if (string.IsNullOrEmpty(name)) return;
        foreach (var t in tags) if (t.name == name) return;
        tags.Add(new TagEntry { name = name, color = color, active = active });
    }
#endif
}
