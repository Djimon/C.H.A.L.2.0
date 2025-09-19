
namespace CHAL.Systems.Inventory
{
    public class InventoryRules
    {
        public static int GetMaxStack(string prefix)
        {
            return prefix switch
            {
                "rune" => 1,
                "remain" => 10000,
                "part" => 1000,
                "module" => 10,
                _ => 100
            };
        }

        public static int GetMaxSlots(string prefix)
        {
            return prefix switch
            {
                "rune" => 20,
                "remain" => 12,
                "part" => 50,
                "module" => 20,
                _ => 30
            };
        }
    }
}
