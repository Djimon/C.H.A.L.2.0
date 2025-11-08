# Save/Load, Versioning & Migration

## Files & Locations
- Windows: `%USERPROFILE%/AppData/LocalLow/CHAL/Saves/`
- Mac: `~/Library/Application Support/CHAL/Saves/`
- Linux: `~/.config/CHAL/Saves/`

**Structure:**
```text
Saves/
  profile_<slotId>/
    meta.json
    player_profile.json
    inventory.json
    research.json
    maps.json
    crafting.json
    version.json
```

## Versioning
`version.json`
```json
{ "schema": 5, "build": "2025.11.08-rc1" }
```
- schema: migration step counter (int, monotonically increasing).
- build: free (SemVer/date code).

## Migration (Pseudocode)
```csharp
int current = LoadVersion();
while(current < SCHEMA_LATEST) {
  switch(current) {
    case 3: Migrate_3_to_4(); break;
    case 4: Migrate_4_to_5(); break;
  }
  current++;
  SaveVersion(current);
}
```

## Encryption & Keys
- Goal: protect against casual editing in builds.
- Recommendation: AES-GCM with key rotation per schema.
- Key storage:
  - Editor: dev key in project (editor flag).
  - Build: split key material (obfuscated chunks + derived from HW/install GUID).
- Integrity: verify GCM tag → on failure: recovery (see below).

## Negative Cases & Recovery
- File missing → create new + defaults, log warning.
- Corrupt/integrity fail → load backup (`*.bak`), else create default profile and log incident.
- Partial migration failed → rollback to backup, error dialog, telemetry event.

## Backups
- Before every migration: copy `*.bak` into the folder.
- Rotate max 3 versions.

## Autosave & Throttling
- Autosave debounce 2s, hard cap 1/30s.
- Long ops async with UI feedback.
