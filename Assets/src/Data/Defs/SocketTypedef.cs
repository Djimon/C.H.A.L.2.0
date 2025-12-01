using System;

namespace CHAL.Data
{

    [Serializable]
    public sealed class SocketTypeDefinition
    {

        public SocketType SocketType { get; }

        public HeroAttribs AllowedStatPrimary { get; }

        public HeroAttribs AllowedStatSecondary { get; }


        public string PrimaryColorHex { get; }

        public string SecondaryColorHex { get; }

        public SocketTypeDefinition(
            SocketType socketType,
            HeroAttribs allowedStatPrimary,
            HeroAttribs allowedStatSecondary,
            string primaryColorHex,
            string secondaryColorHex)
        {
            SocketType = socketType;
            AllowedStatPrimary = allowedStatPrimary;
            AllowedStatSecondary = allowedStatSecondary;
            PrimaryColorHex = primaryColorHex;
            SecondaryColorHex = secondaryColorHex;
        }
    }

    /// <summary>
    /// Static configuration table for all SocketTypes.
    /// 
    /// Mind     → WIL + INT  | Colors: WIL (#26e3e3), INT (#5626e3)
    /// Heart    → STR + WIL  | Colors: STR (#e3263e), WIL (#26e3e3)
    /// Handling → STR + DEX  | Colors: STR (#e3263e), DEX (#9ce326)
    /// Movement → DEX + WIL  | Colors: DEX (#9ce326), WIL (#26e3e3)
    /// Core     → INT + STR  | Colors: INT (#5626e3), STR (#e3263e)
    /// </summary>
    public static class SocketTypeConfig
    {
        // Wichtig: Reihenfolge = Enum-Reihenfolge für schnellen Zugriff per Index.
        private static readonly SocketTypeDefinition[] Definitions =
        {
            // Mind
            new SocketTypeDefinition(
                SocketType.Mind,
                HeroAttribs.WIL,
                HeroAttribs.INT,
                "#26e3e3", // WIL
                "#5626e3"  // INT
            ),

            // Heart
            new SocketTypeDefinition(
                SocketType.Heart,
                HeroAttribs.STR,
                HeroAttribs.WIL,
                "#e3263e", // STR
                "#26e3e3"  // WIL
            ),

            // Core
            new SocketTypeDefinition(
                SocketType.Core,
                HeroAttribs.INT,
                HeroAttribs.STR,
                "#5626e3", // INT
                "#e3263e"  // STR
            ),

            // Handling
            new SocketTypeDefinition(
                SocketType.Handling,
                HeroAttribs.STR,
                HeroAttribs.DEX,
                "#e3263e", // STR
                "#9ce326"  // DEX
            ),

            // Movement
            new SocketTypeDefinition(
                SocketType.Movement,
                HeroAttribs.DEX,
                HeroAttribs.WIL,
                "#9ce326", // DEX
                "#26e3e3"  // WIL
            ),
        };


/// <summary>
/// Retrieves the socket definition for a specified socket type.
/// </summary>
/// <param name="type">The socket type to get the definition for.</param>
/// <returns>The corresponding socket type definition.</returns>
        public static SocketTypeDefinition GetSocketDefiniton(SocketType type)
        {
            var index = (int)type;

            // Defensive check: in case enums change and this table is not updated.
            if (index < 0 || index >= Definitions.Length)
                throw new ArgumentOutOfRangeException(nameof(type),
                    $"No SocketTypeDefinition configured for SocketType '{type}'.");

            return Definitions[index];
        }

/// <summary>
/// Determines if a specific attribute is allowed for a given socket type.
/// </summary>
/// <param name="socketType">The type of the socket to check.</param>
/// <param name="attribute">The attribute to validate.</param>
/// <returns>True if the attribute is allowed; otherwise, false.</returns>
        public static bool AllowsAttribute(SocketType socketType, HeroAttribs attribute)
        {
            var def = GetSocketDefiniton(socketType);
            return def.AllowedStatPrimary == attribute ||
                   def.AllowedStatSecondary == attribute;
        }
    }
}
