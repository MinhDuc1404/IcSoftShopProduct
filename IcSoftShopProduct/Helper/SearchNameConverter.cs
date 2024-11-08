namespace IcSoftShopProduct.Helper
{
    public static class SearchNameConverter
    {
        private static readonly Dictionary<string, string> SearchMappings = new Dictionary<string, string>
        {
            { "ss2024", "S/S 2024" },
            { "lenninxrakkiu", "LENNIN X RAKKIU" },
            { "fw2023", "F/W 2023" },
            { "ss2023", "S/S 2023" },
            // Thêm các cặp ánh xạ khác nếu cần
        };

        public static string ConvertSearchName(string? searchname)
        {
            if (string.IsNullOrEmpty(searchname)) return string.Empty;

            return SearchMappings.TryGetValue(searchname.ToLower(), out var fullName) ? fullName : searchname;
        }
    }
}
