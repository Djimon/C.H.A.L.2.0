# CHAL.Core.SaveSystem

_Automatically generated/updated from `Assets/src/Core/SaveSystem.cs`._

1) Purpose
- Provide a static SaveSystem for saving/loading PlayerProfile data and ResearchSnapshot data via BayatGames.SaveGameFree.
- Resolve per-profile file ids and per-profile research file ids; derive current profile id from file path when needed.
- Log debug/info/warning/error messages around save/load/delete operations and config issues.

2) Public API
- Namespace/module
  - CHAL.Core

- Types
  - public static class SaveSystem
    - Public methods
      - public static void Save(PlayerProfile profile)
      - public static PlayerProfile Load()
      - public static bool DeleteProfileData(string profileId)
      - public static void SaveResearch(string profileId, ResearchSnapshot snap)
      - public static ResearchSnapshot LoadResearch(string profileId)
      - public static bool DeleteResearch(string profileId)
      - public static string CurrentProfileId()

3) Key Behavior & Side Effects
- Configuration
  - ConfigureSaveGame reads GameSaveConfig (Resources.Load<GameSaveConfig>("Config/GameSaveConfig")).
  - If config is missing, logs an error and returns; encoding settings may remain default.
  - Sets SaveGame.Encode and SaveGame.EncodePassword from config (ShouldEncodeRuntime(), encodePassword).
  - SavePath remains the library’s default (persistentDataPath).

- File/id resolution
  - FileId(): uses Cfg.ResolveFileIdRuntime() when config is available; otherwise defaults to "profiles/main/profile.json".
  - ResearchFileId(profileId): returns $"profiles/{profileId}/research_v1.json".
  - CurrentProfileId(): parses FileId() to extract the segment after "profiles/"; returns "main" if parsing fails or empty.

- Saving
  - Save(profile):
    - Returns early if profile is null (logs a warning).
    - Calls ConfigureSaveGame().
    - Calls profile.PrepareInventorySnapshot(); sets profile.LastSaveTime to DateTime.UtcNow.
    - Saves to id = FileId() via SaveGame.Save(id, profile).
    - Logs successful save with the id.

- Loading
  - Load():
    - Calls ConfigureSaveGame().
    - id = FileId(); if SaveGame.Exists(id) is false, logs a warning and returns null.
    - Loads via SaveGame.Load<PlayerProfile>(id); if null, logs error and returns null.
    - Calls p.RestoreInventoriesFromSnapshot().
    - Sets p.profileId = CurrentProfileId().
    - Logs and returns the profile.

- Profile data deletion
  - DeleteProfileData(string profileId):
    - Calls ConfigureSaveGame().
    - pid = (string.IsNullOrWhiteSpace(profileId) ? CurrentProfileId() : profileId).
    - id = ResearchFileId(pid).
    - If SaveGame.Exists(id) is false, returns false.
    - SaveGame.Delete(id); logs deletion; returns true.
    - Note: Deletes the profile’s related data under the research file path (per code).

- Research data saving/loading/deletion
  - SaveResearch(string profileId, ResearchSnapshot snap):
    - ConfigureSaveGame().
    - pid = (string.IsNullOrWhiteSpace(profileId) ? CurrentProfileId() : profileId).
    - id = ResearchFileId(profileId).
    - SaveGame.Save(id, snap ?? new ResearchSnapshot()).
    - Logs.

  - LoadResearch(string profileId):
    - ConfigureSaveGame().
    - pid = (string.IsNullOrWhiteSpace(profileId) ? CurrentProfileId() : profileId).
    - id = ResearchFileId(pid).
    - If !SaveGame.Exists(id), logs warning and returns new ResearchSnapshot().
    - snap = SaveGame.Load<ResearchSnapshot>(id) ?? new ResearchSnapshot().
    - Logs and returns snap.

  - DeleteResearch(string profileId):
    - ConfigureSaveGame().
    - id = ResearchFileId(string.IsNullOrWhiteSpace(profileId) ? "main" : profileId).
    - If !SaveGame.Exists(id), returns false.
    - SaveGame.Delete(id); logs; returns true.

4) Constraints & Failure Modes
- Null handling
  - Save(null) is rejected with a warning; other methods rely on passed-in profileId or current profile id.
- Config availability
  - If GameSaveConfig is not found, ConfigureSaveGame logs an error and returns; encoding flags may stay default.
- File existence
  - Load() returns null if the main profile file does not exist.
  - DeleteProfileData/DeleteResearch return false if the target file does not exist.
- Parsing robustness
  - CurrentProfileId() is resilient: returns "main" if parsing fails or if id is unexpected.
- Data integrity
  - When loading research, missing files yield an empty ResearchSnapshot instead of error.
- Side effects
  - Extensive logging via DebugManager.Log; relies on external DebugManager, SaveGameFree, and Unity.

5) Example
```csharp
// Save a profile
var profile = new PlayerProfile { /* initialize fields */ };
SaveSystem.Save(profile);

// Load the current profile
var loaded = SaveSystem.Load();

// Save research for a specific profile
var snap = new ResearchSnapshot();
SaveSystem.SaveResearch("player1", snap);

// Load research for a specific profile
var loadedSnap = SaveSystem.LoadResearch("player1");
```

6) Unknowns
- Details of PlayerProfile and ResearchSnapshot structures beyond used methods (PrepareInventorySnapshot, RestoreInventoriesFromSnapshot, profileId field, etc.).
- Exact behavior of BayatGames.SaveGameFree (e.g., fault modes on disk I/O, compression, encryption semantics beyond SaveGame.Encode/EncodePassword).
- Implementation specifics of DebugManager.Log and its DebugLevel semantics.
- The full contents and behavior of GameSaveConfig (ResolveFileIdRuntime, ShouldEncodeRuntime, encodePassword).
- Any threading or async implications of SaveGame calls (not stated in this file).

