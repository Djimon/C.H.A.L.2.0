# Assets/src/Core/SaveSystem.cs

_Automatically generated/updated from `Assets/src/Core/SaveSystem.cs`._

# Purpose
- Defines a static `SaveSystem` for managing player profiles and research snapshots in a game.

# Public API
- Namespace: `CHAL.Core`
- Types
  - **static class** `SaveSystem`
    - **Public methods**
      - `void Save(PlayerProfile profile)`
        - Saves the specified player profile to the game storage.
      - `PlayerProfile Load()`
        - Loads the player profile from the save game file, or returns null if no file exists.
      - `bool DeleteProfileData(string profileId)`
        - Deletes the specified profile's data; returns true if successful.
      - `void SaveResearch(string profileId, ResearchSnapshot snap)`
        - Saves a research snapshot associated with the specified profile ID.
      - `ResearchSnapshot LoadResearch(string profileId)`
        - Loads a research snapshot based on the provided profile ID.
      - `bool DeleteResearch(string profileId)`
        - Deletes the research data associated with the specified profile ID; returns true if successful.
      - `string CurrentProfileId()`
        - Retrieves the current profile ID from a file path, returning "main" if no valid ID is found.

# Key Behavior & Side Effects
- Configures save game settings based on `GameSaveConfig` when saving or loading profiles and research.
- Logs warnings and errors if profiles or research data cannot be found or loaded.
- Automatically uses the current profile ID if none is specified for saving or loading research snapshots.

# Constraints & Failure Modes
- Handles null profiles in `Save` method by logging a warning and returning early.
- Returns null in `Load` if no save file exists.
- Returns an empty `ResearchSnapshot` if no research file exists during loading.

# Example
```csharp
PlayerProfile profile = new PlayerProfile();
SaveSystem.Save(profile);

PlayerProfile loadedProfile = SaveSystem.Load();
ResearchSnapshot research = SaveSystem.LoadResearch("main");
```

# Unknowns
- The structure and contents of `PlayerProfile` and `ResearchSnapshot` are not defined in this file.
- The implementation details of `SaveGame` and `DebugManager` are not provided.

