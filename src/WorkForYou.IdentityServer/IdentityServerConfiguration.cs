using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace WorkForYou.IdentityServer;

public static class IdentityServerConfiguration
{
    public static IEnumerable<ApiResource> GetApiResources() =>
        new List<ApiResource>
        {
            new() {Name = "WebApi", Scopes = {"WebApi"}}
        };

    public static IEnumerable<ApiScope> GetApiScopes() =>
        new List<ApiScope>
        {
            new() {Name = "WebApi"}
        };

    public static IEnumerable<IdentityResource> GetIdentityResources() =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };

    public static IEnumerable<Client> GetClients() =>
        new List<Client>
        {
            new()
            {
                ClientId = "web_ui_client_id",
                ClientSecrets = {new("web_ui_client_secret".ToSha256())},

                AllowedGrantTypes = GrantTypes.Code,
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "WebApi"
                },

                AllowOfflineAccess = true,
                RequireConsent = false,

                RedirectUris = {"https://localhost:7231/signin-oidc"} // WebUI client
            }
        };
}