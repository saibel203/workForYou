using WorkForYou.IdentityServer;
using WorkForYou.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddIdentityServerServices();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseHsts();

app.UseRouting();

app.UseIdentityServer();

app.MapDefaultControllerRoute();

app.Run();
