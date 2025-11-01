# CHAL.Data.ModulePartMapWrapper

_Automatically generated/updated from `Assets/src/Data/DTO/ModulePartMapWrapper.cs`._

1) Purpose
- Serializable DTOs for module-to-parts mapping in CHAL.Data.
- ModulePartMapWrapper holds a public list of ModulePartMapEntry named entries.
- Provides ToDictionary() to convert the entries list into a Dictionary<string, string[]>.

2) Public API
- Namespace/module
  - CHAL.Data
- Types
  - public class ModulePartMapWrapper [System.Serializable]
    - Public fields:
      - public List<ModulePartMapEntry> entries
    - Public methods:
      - public Dictionary<string, string[]> ToDictionary()
  - public class ModulePartMapEntry [System.Serializable]
    - Public fields:
      - public string moduleId
      - public string[] parts

3) Key Behavior & Side Effects
- ToDictionary():
  - Creates a new Dictionary<string, string[]>.
  - Iterates over entries and assigns dict[e.moduleId] = e.parts.
  - Returns the populated dictionary.
- Behavior notes:
  - No null checks; possible exceptions:
    - NullReferenceException if entries is null.
    - ArgumentNullException if any e.moduleId is null (null keys not allowed).
  - e.parts may be null; dictionary value can be null.
  - Duplicate moduleId entries will overwrite earlier ones (last-wins).

4) Constraints & Failure Modes
- Guards: none present for null entries, null moduleId, or null elements.
- Threading/async: none; method is synchronous.
- Performance: O(n) time, O(n) space for the resulting dictionary.
- Unity serialization: both classes are marked [System.Serializable], enabling Unity to serialize the wrapper and its entries.

5) Example
```csharp
var wrapper = new CHAL.Data.ModulePartMapWrapper
{
    entries = new List<CHAL.Data.ModulePartMapEntry>
    {
        new CHAL.Data.ModulePartMapEntry { moduleId = "modA", parts = new string[] { "p1", "p2" } },
        new CHAL.Data.ModulePartMapEntry { moduleId = "modB", parts = new string[] { "p3" } }
    }
};

var dict = wrapper.ToDictionary();
// dict["modA"] -> ["p1", "p2"]
// dict["modB"] -> ["p3"]
```

6) Unknowns
- How this DTO is intended to be populated (UI, JSON, or runtime data source) is not specified.
- Behavior with null entries or malformed data is not defined beyond implicit exceptions.
