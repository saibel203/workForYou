using AspNetCoreHero.ToastNotification.Extensions;
using Microsoft.Extensions.Options;
using WorkForYou.Core;
using WorkForYou.Infrastructure;
using WorkForYou.Infrastructure.DatabaseContext;
using WorkForYou.WebUI;
using WorkForYou.WebUI.Extensions;
using WorkForYou.WebUI.Hubs;

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

app.UsePageErrors();

app.UseNotyf();
app.UseHttpsRedirection();
app.UseHsts();

app.UseStaticFiles();
app.UseRouting();

app.UseRequestLocalization(localizationOptions);

app.UseSession();

app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
app.MapHub<ChatHub>("/hubs/chatHub");

app.Run();