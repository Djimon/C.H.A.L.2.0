# global.RuneForgeConfigEditor

_Automatically generated/updated from `Assets/src/Editor/RunForgeConfigEditor.cs`._

1) Purpose
- Defines a Unity Editor CustomEditor for RuneForgeConfig to customize its inspector UI.
- Uses ItemRegistry to populate dropdowns for remains and runes; warns if none found.
- Manages and renders editable collections: config.entries, entry.remain, and entry.runes (RuneChance items), with add/remove capabilities and foldout state.

2) Public API
- Type: public class RuneForgeConfigEditor : Editor
  - Public methods
    - public override void OnInspectorGUI()
      - Renders and handles the custom inspector UI for RuneForgeConfig
      - Mutates the target RuneForgeConfig (entries, remains, runes, weights, etc.)
      - Triggers EditorUtility.SetDirty(config) when GUI.changed

3) Key Behavior & Side Effects
- OnInspectorGUI flow
  - Casts target to RuneForgeConfig as config.
  - Ensures config.entries is non-null (initializes to new List<RuneForgeEntry>()).
  - Retrieves remains and runes lists from ItemRegistry.Instance.GetAllItemsByType("remains") and ...("rune"), converting to List.
  - If remains.Count == 0 or runes.Count == 0, shows a warning HelpBox.
  - For each entry in config.entries:
    - Ensures entry.runes is non-null.
    - Synchronizes runeFoldouts length with entry.runes.Count (append true as needed).
    - Renders a boxed vertical group:
      - Remain dropdown:
        - Determines remainIndex from entry.remain in the remains list.
        - Presents Popup("Remain", remainIndex, remains[i].itemId array).
        - Updates entry.remain when a selection is made.
      - For each RuneChance in entry.runes:
        - Renders a Foldout titled "RuneChance N" (N = index+1) with persistent state.
        - If expanded:
          - Rune dropdown:
            - Determines runeIndex from rc.rune in runes.
            - Presents Popup("Rune", runeIndex, runes[i].itemId array).
            - Updates rc.rune when a selection is made.
          - Weight slider: rc.weight = Slider("Weight", rc.weight, 0f, 1f).
          - Remove Rune button:
            - Removes the RuneChance from entry.runes and its foldout entry.
            - Decrements loop index and indentation accordingly.
      - Spacing and action row:
        - + Rune button: appends new RuneChance() to entry.runes and adds a foldout entry.
        - - Remain button: removes the current config.entries[e], decrements e, and breaks out of the loop.
  - End of per-entry rendering.
  - + Remain button (outside per-entry loop):
    - Adds a new RuneForgeEntry { runes = new List<RuneChance>() } to config.entries.
  - If GUI.changed, marks config dirty with EditorUtility.SetDirty(config).

4) Constraints & Failure Modes
- Editor-only: code compiled only under UNITY_EDITOR.
- Requires ItemRegistry to be initialized to populate remains/runes; otherwise dropdowns may be empty.
- Null handling:
  - config.entries is auto-initialized if null.
  - entry.runes and rc (RuneChance) entries are guarded; null entries are skipped.
- List synchronization:
  - runeFoldouts length synced to entry.runes.Count; relies on manual adjustments when removing elements.
  - Removing a Rune or an Entry updates related lists and may alter the active loop index.
- State persistence:
  - Changes are persisted by calling EditorUtility.SetDirty(config) when GUI.changed is true.
- Runtime implications:
  - This is editor tooling; behavior does not affect runtime unless the edited ScriptableObject is saved/used in play mode.
- Potential edge cases not guarded against:
  - Duplicated itemIds in remains/runes lists are not addressed.
  - Non-initialized ItemRegistry data could yield empty dropdowns.
  - No explicit validation beyond UI-level constraints (e.g., negative indices are guarded by checks).

5) Example
- Not provided (no derivable minimal runnable snippet directly from this file).

6) Unknowns
- Exact definitions and members of:
  - RuneForgeConfig
  - RuneForgeEntry
  - RuneChance
  - ItemRegistry, Item type (fields like itemId)
- Expected semantics of remains vs rune items beyond itemId usage.
- Any additional editor tooling or integration points outside this file.

