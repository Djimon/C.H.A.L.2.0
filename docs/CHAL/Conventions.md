# Conventions (Cross-Cutting)

## Logging Policy
- Never use `UnityEngine.Debug.*` → always use `DebugManager`.
- Required tags as second parameter: `System`, `UI`, `Save`, `Research`.
- Levels: Info, Warn, Error.
- Example:
```csharp
DebugManager.Info("Profile migrated to schema 5","Save");
```

## Error Handling
- Recoverable → avoid exceptions, return error types + retry strategies.
- Non-recoverable → targeted dialog + incident log + backup restore.
- No silent failures.

## Performance Budget
- Update cost: services must not be heavy per frame (prefer events over polling).
- Use pooling for enemies/projectiles.
- Reduce GC pressure: reuse buffers/DTOs.
- Profiling checklist per release candidate.

## Test Strategy
- **EditorTests**: registries, validation, CSV schemas.
- **PlayMode**: state flow, guards (`NoPendingLoot`), reward apply.
- **Golden Data**: fixed seeds for loot/rolls.
- **Migration Tests**: roundtrip older schemas.

## Telemetry (optional)
- Session ID, critical faults, migration results (opt-in).
