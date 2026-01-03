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
        - Saves a research snapshot associated with the specified profile ID. If the profile ID is empty, the current profile ID is used.
      - `ResearchSnapshot LoadResearch(string profileId)`
        - Loads a research snapshot based on the provided profile ID. If the profile ID is empty, the current profile ID is used.
      - `bool DeleteResearch(string profileId)`
        - Deletes the research data associated with the specified profile ID; returns true if successful.
      - `void SaveStatistics(string profileId, StatisticsSnapshot snapshot)`
        - Saves a statistics snapshot associated with the specified profile ID. If the snapshot is null, a new empty snapshot is saved.
      - `StatisticsSnapshot LoadStatistics(string profileId)`
        - Loads a statistics snapshot based on the provided profile ID. Returns an empty snapshot if no statistics file exists.
      - `void SaveInventories(string profileId, List<InventorySnapshot> snapshots)`
        - Saves the inventory snapshots for a specified profile ID in a separate file.
      - `List<InventorySnapshot> LoadInventories(string profileId)`
        - Loads inventory snapshots for a given profile ID from the separate inventory file. Returns an empty list if no file exists.
      - `bool DeleteInventories(string profileId)`
        - Deletes the inventory save file for the given profile ID.
      - `string CurrentProfileId()`
        - Retrieves the current profile ID from a file path, returning "main" if no valid ID is found.

# Key Behavior & Side Effects
- Configures save game settings based on `GameSaveConfig` when saving or loading profiles, research, and statistics.
- Logs warnings and errors if profiles, research data, or statistics cannot be found or loaded.
- Automatically uses the current profile ID if none is specified for saving or loading research snapshots or statistics.
- Logs inventory data during profile loading for debugging purposes.

# Constraints & Failure Modes
- Handles null profiles in `Save` method by logging a warning and returning early.
- Returns null in `Load` if no save file exists.
- Returns an empty `ResearchSnapshot` if no research file exists during loading.
- Returns an empty `StatisticsSnapshot` if no statistics file exists during loading.
- Returns an empty list in `LoadInventories` if no inventory file exists.

# Example
```csharp
PlayerProfile profile = new PlayerProfile();
SaveSystem.Save(profile);

PlayerProfile loadedProfile = SaveSystem.Load();
ResearchSnapshot research = SaveSystem.LoadResearch("main");
StatisticsSnapshot stats = SaveSystem.LoadStatistics("main");
List<InventorySnapshot> inventories = SaveSystem.LoadInventories("main");
```

# Unknowns
- The structure and contents of `PlayerProfile`, `ResearchSnapshot`, `StatisticsSnapshot`, and `InventorySnapshot` are not defined in this file.
- The implementation details of `SaveGame` and `DebugManager` are not provided.
