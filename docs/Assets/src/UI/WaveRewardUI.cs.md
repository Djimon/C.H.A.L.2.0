# Assets/src/UI/WaveRewardUI.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines the `WaveRewardUI` class, which manages the UI for wave rewards in the game.

## Public API
- Namespace: None
- Types
  - `public class WaveRewardUI : IngameUI`
    - Public fields/properties: None
    - Public methods:
      - `public void populateText(bool succeded)`
        - Updates the details text based on the success of the wave.

## Key Behavior & Side Effects
- `Awake()`: Initializes UI elements and sets up button click event handlers.
- `populateText(bool succeded)`: Changes the text and color of the `detailsText` based on the success parameter; logs a message to the debug manager.

## Constraints & Failure Modes
- Assumes the presence of a `UIDocument` component for UI elements.
- Uses `FindFirstObjectByType<MapManager>()` to locate the `MapManager`, which may fail if not present.

## Example
```csharp
WaveRewardUI waveRewardUI = new WaveRewardUI();
waveRewardUI.populateText(true); // Updates text to "Successful!" with success color.
```

## Unknowns
- No information on the behavior of `IngameUI` or `MapManager`.
- No details on the context in which `WaveRewardUI` is used or instantiated.
```
