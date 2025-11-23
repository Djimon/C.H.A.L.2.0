# Assets/src/Data/Defs/SocketTypedef.cs

_Automatically generated/updated from `Assets/src/Data/Defs/SocketTypedef.cs`._

# Purpose
- Defines the `SocketTypeDefinition` class representing the configuration for different socket types.
- Provides a static configuration table `SocketTypeConfig` for all socket types and methods to retrieve definitions and validate attributes.

# Public API
- Namespace: `CHAL.Data`
- Types
  - **sealed class** `SocketTypeDefinition`
    - **Public properties:**
      - `SocketType SocketType { get; }` - The type of the socket.
      - `HeroAttribs AllowedStatPrimary { get; }` - The primary attribute allowed for this socket type.
      - `HeroAttribs AllowedStatSecondary { get; }` - The secondary attribute allowed for this socket type.
      - `string PrimaryColorHex { get; }` - Hex color code for the primary color.
      - `string SecondaryColorHex { get; }` - Hex color code for the secondary color.
    - **Public methods:**
      - `SocketTypeDefinition(SocketType socketType, HeroAttribs allowedStatPrimary, HeroAttribs allowedStatSecondary, string primaryColorHex, string secondaryColorHex)` - Constructor to initialize a socket type definition.
  
  - **static class** `SocketTypeConfig`
    - **Public methods:**
      - `static SocketTypeDefinition GetSocketDefiniton(SocketType type)` - Retrieves the socket type definition for the specified socket type.
      - `static bool AllowsAttribute(SocketType socketType, HeroAttribs attribute)` - Checks if the specified attribute is allowed for the given socket type.

# Key Behavior & Side Effects
- `GetSocketDefiniton(SocketType type)` throws `ArgumentOutOfRangeException` if the provided socket type is not valid.
- `AllowsAttribute(SocketType socketType, HeroAttribs attribute)` returns `true` if the attribute is allowed for the specified socket type, otherwise `false`.

# Constraints & Failure Modes
- The `GetSocketDefiniton` method includes a defensive check for valid enum values to prevent out-of-bounds access.
- No threading or async considerations are present in this file.

# Example
```csharp
var socketDef = SocketTypeConfig.GetSocketDefiniton(SocketType.Mind);
bool isAllowed = SocketTypeConfig.AllowsAttribute(SocketType.Mind, HeroAttribs.WIL);
```

# Unknowns
- None.
