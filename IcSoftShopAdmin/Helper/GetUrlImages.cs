namespace IcSoftShopAdmin.Helper
{
    public class GetUrlImages
    {
        private readonly string _domainUrl;
        
        public GetUrlImages(IConfiguration configuration)
        {
            _domainUrl = configuration.GetSection("AppSettings:DomainUrl").Value;
        }

        public string GetUrl(string url)
        {
          
            if (string.IsNullOrWhiteSpace(url))
                return _domainUrl;

            return  $"{_domainUrl.TrimEnd('/')}/{url.TrimStart('/')}";
        }
    }
}
