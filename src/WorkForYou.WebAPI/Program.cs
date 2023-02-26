using Microsoft.Extensions.Options;
using WorkForYou.Infrastructure;
using WorkForYou.WebAPI;
using WorkForYou.WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebApiServices(builder.Configuration);

var app = builder.Build();
var environment = app.Environment;
var localizationOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;

app.UseExceptionMiddlewareHandler(environment);

app.UseHttpsRedirection();
app.UseHsts();
app.UseRouting();

app.UseRequestLocalization(localizationOptions);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
