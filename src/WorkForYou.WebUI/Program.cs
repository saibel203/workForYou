using AspNetCoreHero.ToastNotification.Extensions;
using Microsoft.Extensions.Options;
using WorkForYou.Core;
using WorkForYou.Data;
using WorkForYou.Data.DatabaseContext;
using WorkForYou.WebUI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebUiServices(builder.Configuration);

var app = builder.Build();
var localizationOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();

    var init = scope.ServiceProvider.GetRequiredService<SeedDbContext>();
    await init.InitialiseDatabaseAsync();
    await init.SeedDataAsync();
}

app.UseNotyf();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseRequestLocalization(localizationOptions);

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

app.Run();