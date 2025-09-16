namespace CHAL.Data
{
    public readonly struct ItemKey
    {
        public readonly string Category;
        public readonly string Id;
        public ItemKey(string category, string id) { Category = category; Id = id; }
        public static bool TryParse(string s, out ItemKey key)
        {
            key = default;
            if (string.IsNullOrWhiteSpace(s)) return false;
            var parts = s.Split(':');
            if (parts.Length != 2) return false;
            key = new ItemKey(parts[0], parts[1]); return true;
        }
        public override string ToString() => $"{Category}:{Id}";
    }
}