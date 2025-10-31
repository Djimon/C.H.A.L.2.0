# CHAL.Core.SaveSystem

_Automatically generated/updated from `Assets/src/Core/SaveSystem.cs`._

# Purpose
- Defines a static `SaveSystem` for managing player profiles and research data.
- Provides methods for saving, loading, and deleting player profiles and research snapshots.

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
- Configures save game settings based on `GameSaveConfig`.
- Logs warnings/errors when profiles or research data cannot be found or loaded.
- Updates the last save time when saving a profile.
- Returns an empty `ResearchSnapshot` if no research data exists.

# Constraints & Failure Modes
- Handles null profiles in `Save` and logs a warning.
- Returns `null` if loading a profile fails or if the file does not exist.
- Uses `CurrentProfileId` to determine the profile ID when none is provided.

# Example
```csharp
PlayerProfile profile = new PlayerProfile();
SaveSystem.Save(profile);
PlayerProfile loadedProfile = SaveSystem.Load();
```

# Unknowns
- The structure and contents of `PlayerProfile` and `ResearchSnapshot` are not defined in this file.
- The implementation details of `SaveGame` and `DebugManager` are not provided.

