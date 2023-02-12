using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using WorkForYou.Infrastructure.Resources;

namespace WorkForYou.Infrastructure;

public class MultiLanguageIdentityErrorDescriber : IdentityErrorDescriber
{
    private readonly IStringLocalizer _stringLocalization;

    public MultiLanguageIdentityErrorDescriber(IStringLocalizerFactory factory)
    {
        var type = typeof(IdentityErrors);
        var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName!);
        _stringLocalization = factory.Create("IdentityErrors", assemblyName.Name!);
    }

    public override IdentityError DuplicateEmail(string email)
    {
        return new()
        {
            Code = nameof(DuplicateEmail),
            Description = string.Format(_stringLocalization["Email '{0}' is already taken"], email)
        };
    }

    public override IdentityError DuplicateUserName(string username)
    {
        return new()
        {
            Code = nameof(DuplicateUserName),
            Description = string.Format(_stringLocalization["Username '{0}' is already taken"], username)
        };
    }
}