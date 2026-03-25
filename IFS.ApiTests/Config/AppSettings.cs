namespace IFS.ApiTests.Config
{
    public class AppSettings
    {
        public ApiSettings ApiSettings { get; set; }
    }

    public class ApiSettings
    {
        public string BaseUrl { get; set; }
        public int TimeoutSeconds { get; set; }
    }
}