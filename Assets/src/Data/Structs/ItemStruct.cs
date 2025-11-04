namespace CHAL.Data
{
    public readonly struct ItemKey
    {
        public readonly string Category;
        public readonly string Id;
        public ItemKey(string category, string id) { Category = category; Id = id; }
/// <summary>
/// Tries to parse a string into an ItemKey object.
/// </summary>
/// <param name="s">The string to parse.</param>
/// <param name="key">The resulting ItemKey if parsing is successful.</param>
/// <returns>True if parsing succeeded; otherwise, false.</returns>
        public static bool TryParse(string s, out ItemKey key)
        {
            key = default;
            if (string.IsNullOrWhiteSpace(s)) return false;
            var parts = s.Split(':');
            if (parts.Length != 2) return false;
            key = new ItemKey(parts[0], parts[1]); return true;
        }
/// <summary>
/// Returns a string representation of the object, including its category and ID.
/// </summary>
/// <returns>A formatted string of the category and ID.</returns>
        public override string ToString() => $"{Category}:{Id}";
    }
}
