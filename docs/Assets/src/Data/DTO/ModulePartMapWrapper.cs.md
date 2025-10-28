# Assets/src/Data/DTO/ModulePartMapWrapper.cs

_Automatic generated/updated._

```markdown
# Purpose
- Defines data structures for mapping module IDs to part arrays.
- Provides a method to convert the list of mappings into a dictionary.

# Public API
- Namespace: `CHAL.Data`
- Types
  - `public class ModulePartMapWrapper`
    - Public fields/properties:
      - `public List<ModulePartMapEntry> entries;` - List of module-part mappings.
    - Public methods:
      - `public Dictionary<string, string[]> ToDictionary();` - Converts entries to a dictionary mapping module IDs to part arrays.
  
  - `public class ModulePartMapEntry`
    - Public fields/properties:
      - `public string moduleId;` - Identifier for the module.
      - `public string[] parts;` - Array of parts associated with the module ID.

# Key Behavior & Side Effects
- The `ToDictionary` method iterates through `entries` and constructs a dictionary where each key is a `moduleId` and the value is the corresponding `parts` array.

# Constraints & Failure Modes
- Assumes `entries` is not null; behavior is undefined if it is.
- No explicit error handling for duplicate `moduleId` entries in `ToDictionary`.

# Example
```csharp
var wrapper = new ModulePartMapWrapper();
wrapper.entries = new List<ModulePartMapEntry>
{
    new ModulePartMapEntry { moduleId = "module1", parts = new[] { "partA", "partB" } },
    new ModulePartMapEntry { moduleId = "module2", parts = new[] { "partC" } }
};
var dictionary = wrapper.ToDictionary();
```

# Unknowns
- None.
```
