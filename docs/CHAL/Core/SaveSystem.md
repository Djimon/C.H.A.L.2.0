# Assets/src/Core/SaveSystem.cs

_Automatically generated/updated from `Assets/src/Core/SaveSystem.cs`._

# Purpose
- Defines a static `SaveSystem` for managing player profiles, research snapshots, and statistics in a game.

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
      - `void SaveStatistics(string profileId, StatisticsSnapshot snapshot)`
        - Saves a statistics snapshot associated with the specified profile ID.
      - `StatisticsSnapshot LoadStatistics(string profileId)`
        - Loads a statistics snapshot based on the provided profile ID.
      - `string CurrentProfileId()`
        - Retrieves the current profile ID from a file path, returning "main" if no valid ID is found.

# Key Behavior & Side Effects
- Configures save game settings based on `GameSaveConfig` when saving or loading profiles, research, and statistics.
- Logs warnings and errors if profiles, research data, or statistics cannot be found or loaded.
- Automatically uses the current profile ID if none is specified for saving or loading research snapshots or statistics.

# Constraints & Failure Modes
- Handles null profiles in `Save` method by logging a warning and returning early.
- Returns null in `Load` if no save file exists.
- Returns an empty `ResearchSnapshot` if no research file exists during loading.
- Returns an empty `StatisticsSnapshot` if no statistics file exists during loading.

# Example
```csharp
PlayerProfile profile = new PlayerProfile();
SaveSystem.Save(profile);

PlayerProfile loadedProfile = SaveSystem.Load();
ResearchSnapshot research = SaveSystem.LoadResearch("main");
StatisticsSnapshot stats = SaveSystem.LoadStatistics("main");
```

# Unknowns
- The structure and contents of `PlayerProfile`, `ResearchSnapshot`, and `StatisticsSnapshot` are not defined in this file.
- The implementation details of `SaveGame` and `DebugManager` are not provided.
