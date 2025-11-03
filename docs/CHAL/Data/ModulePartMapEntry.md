# CHAL.Data.ModulePartMapEntry

_Automatically generated/updated from `Assets/src/Data/DTO/ModulePartMapWrapper.cs`._

1) Purpose
- Defines serializable data structures in CHAL.Data for mapping module IDs to their parts.
- ModulePartMapWrapper holds a list of ModulePartMapEntry and can convert it to a Dictionary<string, string[]> via ToDictionary().
- ModulePartMapEntry represents a single mapping with moduleId and parts fields.

2) Public API
- Namespace/module
  - CHAL.Data
- Types
  - public class ModulePartMapWrapper
    - public List<ModulePartMapEntry> entries
    - public Dictionary<string, string[]> ToDictionary()
      - Returns: a new dictionary mapping moduleId to parts
      - Side effects: builds and returns a new Dictionary; may override values for duplicate moduleId keys
  - public class ModulePartMapEntry
    - public string moduleId
    - public string[] parts

3) Key Behavior & Side Effects
- ToDictionary:
  - Creates a new Dictionary<string, string[]>.
  - Iterates over entries and assigns dict[e.moduleId] = e.parts.
  - Returns the constructed dictionary.
- Assumptions/risks:
  - If entries is null, calling ToDictionary will throw NullReferenceException.
  - If any entry is null, or if e.moduleId is null, behavior is undefined (likely exceptions: NullReferenceException or ArgumentNullException).
  - If an entry's parts is null, the dictionary value will be null for that key.
  - Duplicate moduleId keys result in the last entry winning (overwrites earlier ones).

4) Constraints & Failure Modes
- Null handling: no guards for entries, individual entries, or moduleId null values.
- Threading: not thread-safe; no synchronization.
- Serialization: marked as [System.Serializable] to support Unity serialization; public fields are serialized.
- Performance: O(n) time and O(n) memory relative to the number of entries.
- Values: dictionary values are string[]; may be null if corresponding parts is null.

5) Example
```csharp
var wrapper = new CHAL.Data.ModulePartMapWrapper
{
    entries = new List<CHAL.Data.ModulePartMapEntry>
    {
        new CHAL.Data.ModulePartMapEntry { moduleId = "ModA", parts = new[] { "Part1", "Part2" } },
        new CHAL.Data.ModulePartMapEntry { moduleId = "ModB", parts = new[] { "Part3" } }
    }
};

var dict = wrapper.ToDictionary();
// dict["ModA"] -> ["Part1", "Part2"]
// dict["ModB"] -> ["Part3"]
```

6) Unknowns
- No constructors defined beyond default; behavior relies on default initialization.
- No validation on entry content beyond Unity serialization semantics.
- Usage context (e.g., how entries are populated) is not specified in this file.

