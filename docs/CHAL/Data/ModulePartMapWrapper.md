# CHAL.Data.ModulePartMapWrapper

_Automatically generated/updated from `Assets/src/Data/DTO/ModulePartMapWrapper.cs`._

# Purpose
- Defines data structures for mapping module IDs to part arrays.

# Public API
- Namespace: `CHAL.Data`
- Types
  - **public class** `ModulePartMapWrapper`
    - **public List<ModulePartMapEntry>** `entries` - List of module part mappings.
    - **public Dictionary<string, string[]>** `ToDictionary()` - Converts entries to a dictionary mapping module IDs to part arrays.
  
  - **public class** `ModulePartMapEntry`
    - **public string** `moduleId` - Identifier for the module.
    - **public string[]** `parts` - Array of parts associated with the module.

# Key Behavior & Side Effects
- `ToDictionary()` method iterates over `entries` to create a dictionary, mapping each `moduleId` to its corresponding `parts`.

# Constraints & Failure Modes
- No explicit null or empty handling is present for `entries` or its elements.
- Assumes `entries` is initialized before calling `ToDictionary()`.

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
- No information on how `ModulePartMapWrapper` is used within the broader application context.

