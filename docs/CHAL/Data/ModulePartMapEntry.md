# CHAL.Data.ModulePartMapEntry

_Automatically generated/updated from `Assets/src/Data/DTO/ModulePartMapWrapper.cs`._

# Purpose
- Defines data structures for mapping module IDs to their corresponding parts.

# Public API
- Namespace: CHAL.Data
- Types
  - [Serializable] class ModulePartMapWrapper
    - Public fields/properties:
      - List<ModulePartMapEntry> entries: Collection of module part mappings.
    - Public methods:
      - Dictionary<string, string[]> ToDictionary(): Converts entries to a dictionary mapping module IDs to parts.

  - [Serializable] class ModulePartMapEntry
    - Public fields/properties:
      - string moduleId: Identifier for the module.
      - string[] parts: Array of parts associated with the module.

# Key Behavior & Side Effects
- The `ToDictionary` method creates a new dictionary from the `entries` list, mapping each `moduleId` to its corresponding `parts`.

# Constraints & Failure Modes
- No explicit guards or null handling noted.
- Assumes `entries` is initialized before calling `ToDictionary`.

# Example
```csharp
var wrapper = new ModulePartMapWrapper();
wrapper.entries = new List<ModulePartMapEntry>
{
    new ModulePartMapEntry { moduleId = "module1", parts = new[] { "partA", "partB" } }
};
var dictionary = wrapper.ToDictionary();
```

# Unknowns
- No information on threading or performance considerations.

