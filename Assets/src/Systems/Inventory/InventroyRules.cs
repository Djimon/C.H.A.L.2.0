
namespace CHAL.Systems.Inventory
{
/// <summary>
/// Provides rules and methods for managing inventory items.
/// </summary>
    public class InventoryRules
    {
/// <summary>
/// Gets the maximum number of slots based on the given prefix.
/// </summary>
/// <param name="prefix">The prefix to determine the maximum stack size.</param>
/// <returns>The maximum number of slots for the specified prefix.</returns>
        public static int GetMaxStack(string prefix)
        {
            return prefix switch
            {
                "rune" => 1,
                "remains" => 10000,
                "part" => 250,
                "module" => 10,
                _ => 100
            };
        }

/// <summary>
/// Gets the maximum number of slots based on the given prefix.
/// </summary>
/// <param name="prefix">The prefix to determine the maximum slots.</param>
/// <returns>The maximum number of slots for the specified prefix.</returns>
        public static int GetMaxSlots(string prefix)
        {
            return prefix switch
            {
                "rune" => 20,
                "remain" => 12,
                "part" => 100,
                "module" => 30,
                _ => 30
            };
        }
    }
}
