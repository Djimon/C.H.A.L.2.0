# Assets/src/Systems/Localization/LocalizationDict.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines a localization entry and a dictionary for localization entries.

## Public API
- Namespace: `CHAL.Systems.Localization`
- Types
  - `public class LocalizationEntry`
    - Public fields/properties:
      - `public string key` - The key for the localization entry.
      - `public string value` - The value for the localization entry.
  - `public class LocalizationDict`
    - Public fields/properties:
      - `public List<LocalizationEntry> entries` - List of localization entries.
    - Public methods:
      - `public Dictionary<string, string> ToDictionary()` - Converts the list of entries to a dictionary, ignoring entries with null or empty keys.

## Key Behavior & Side Effects
- `ToDictionary()` creates a new dictionary from the `entries` list, filtering out any entries with null or empty keys.

## Constraints & Failure Modes
- Handles null or empty keys by not including them in the resulting dictionary.
```
