# Assets/src/Core/SaveSystem.cs

_Automatic generated/updated._

```markdown
# Purpose
- Defines a static `SaveSystem` for managing player profiles and research data.
- Provides methods for saving, loading, and deleting player profile and research data.

# Public API
- Namespace: `CHAL.Core`
- Types
  - `static class SaveSystem`
    - Public methods:
      - `void Save(PlayerProfile profile)`
        - Saves the provided `PlayerProfile`.
      - `PlayerProfile Load()`
        - Loads and returns the `PlayerProfile`.
      - `bool DeleteProfileData(string profileId)`
        - Deletes the profile data for the specified `profileId`.
      - `void SaveResearch(string profileId, ResearchSnapshot snap)`
        - Saves the research snapshot for the specified `profileId`.
      - `ResearchSnapshot LoadResearch(string profileId)`
        - Loads and returns the research snapshot for the specified `profileId`.
      - `bool DeleteResearch(string profileId)`
        - Deletes the research data for the specified `profileId`.
      - `string CurrentProfileId()`
        - Returns the current profile ID.

# Key Behavior & Side Effects
- `Save(PlayerProfile profile)`:
  - Logs a warning if `profile` is null.
  - Prepares the inventory snapshot and updates the last save time.
- `Load()`:
  - Logs a warning if no save file exists.
  - Logs an error if loading the profile fails.
- `DeleteProfileData(string profileId)`:
  - Deletes the profile data and logs the action.
- `SaveResearch(string profileId, ResearchSnapshot snap)`:
  - Saves the research snapshot and logs the action.
- `LoadResearch(string profileId)`:
  - Returns an empty snapshot if no research file exists.
- `DeleteResearch(string profileId)`:
  - Deletes the research data and logs the action.

# Constraints & Failure Modes
- Handles null or empty `profileId` by using the current profile ID.
- Logs errors and warnings for various failure scenarios (e.g., missing files, null profiles).
- Uses `Resources.Load` to fetch configuration, which may fail if the resource is not found.

# Example
```csharp
PlayerProfile profile = new PlayerProfile();
SaveSystem.Save(profile);
PlayerProfile loadedProfile = SaveSystem.Load();
bool deleted = SaveSystem.DeleteProfileData("main");
```

# Unknowns
- The structure and contents of `PlayerProfile` and `ResearchSnapshot` are not defined in this file.
- The implementation details of `SaveGame` and `DebugManager` are not provided.
```
