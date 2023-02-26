using IdentityServer4.Models;

namespace WorkForYou.IdentityServer;

public static class IdentityServerConfiguration
{
    public static IEnumerable<ApiResource> GetApiResources() =>
        new List<ApiResource>
        {
            new()
            {
                
            }
        };

    public static IEnumerable<ApiScope> GetApiScopes() =>
        new List<ApiScope>
        {
            new() {}
        };

    public static IEnumerable<IdentityResource> GetIdentityResources() =>
        new List<IdentityResource>
        {
            new() { }
        };

    public static IEnumerable<Client> GetClients() =>
        new List<Client>
        {
            
        };
}
