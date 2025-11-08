# CHAL.Core.SaveSystem

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
        - Retrieves the current profile ID from a file path.

# Key Behavior & Side Effects
- Configures save game settings based on `GameSaveConfig` when saving or loading profiles and research.
- Logs warnings and errors if profiles or research data cannot be found or loaded.
- Automatically uses the current profile ID if none is provided for saving or loading research snapshots.

# Constraints & Failure Modes
- If `GameSaveConfig` is not found, saving/loading operations will log an error and may not proceed.
- Null or empty profile IDs will default to the current profile ID.
- If a save file does not exist when loading, a warning is logged and a new instance of the respective object is returned.

# Example
```csharp
PlayerProfile profile = new PlayerProfile();
SaveSystem.Save(profile);
PlayerProfile loadedProfile = SaveSystem.Load();
```

# Unknowns
- The structure and contents of `PlayerProfile` and `ResearchSnapshot` are not defined in this file.
- The behavior of `SaveGame` methods is not detailed in this file.

