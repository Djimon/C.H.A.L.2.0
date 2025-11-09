# Assets/src/Data/DTO/ModulePartMapWrapper.cs

_Automatically generated/updated from `Assets/src/Data/DTO/ModulePartMapWrapper.cs`._

# Purpose
- Defines data structures for mapping module IDs to their corresponding parts.

# Public API
- Namespace: CHAL.Data
- Types
  - public class ModulePartMapWrapper
    - Public fields/properties:
      - List<ModulePartMapEntry> entries: Collection of module part mappings.
    - Public methods:
      - Dictionary<string, string[]> ToDictionary(): Converts entries to a dictionary mapping module IDs to their corresponding parts.
  
  - public class ModulePartMapEntry
    - Public fields/properties:
      - string moduleId: Identifier for the module.
      - string[] parts: Array of parts associated with the module ID.

# Key Behavior & Side Effects
- The `ToDictionary` method creates a dictionary where each key is a module ID and the value is an array of parts.

# Constraints & Failure Modes
- No explicit guards or null handling is present in the code.
- Assumes that `entries` is initialized before calling `ToDictionary`.

# Example
```csharp
var wrapper = new ModulePartMapWrapper
{
    entries = new List<ModulePartMapEntry>
    {
        new ModulePartMapEntry { moduleId = "module1", parts = new[] { "partA", "partB" } },
        new ModulePartMapEntry { moduleId = "module2", parts = new[] { "partC" } }
    }
};

var dictionary = wrapper.ToDictionary();
```

# Unknowns
- No information on how `ModulePartMapWrapper` is used in the broader application context.

