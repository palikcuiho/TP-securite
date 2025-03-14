namespace IdentityServer.Models
{
    public class IdentityServerConfiguration(IConfigurationRoot root, string path)
        : ConfigurationSection(root, path)
    {
        public string ClientId { get; set; } = null!;
        public List<string> Scopes { get; set; } = null!;
        public string TokenEndpoint { get; set; } = null!;
        public int LockoutThreshold { get; set; }
        public double LockoutDuration { get; set; }
    }
}
