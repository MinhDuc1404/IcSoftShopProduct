namespace IcSoftShopAdmin.Helper
{
    public static class GetUrlImages
    {
        public static string GetProductImagePath()
        {
            return Path.Combine("..", "IcSoftShopProduct", "wwwroot").Replace("\\", "/"); ;
        }
    }
}
