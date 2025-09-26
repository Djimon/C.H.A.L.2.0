
namespace CHAL.Systems.Inventory
{
    public class InventoryRules
    {
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
