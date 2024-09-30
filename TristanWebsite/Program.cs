using TristanWebsite;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using TristanWebsite.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddAzureAppConfiguration(Environment.GetEnvironmentVariable("ConnectionString")!);

var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("VaultUri")!);
builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());

var secretClient = new SecretClient(keyVaultEndpoint, new DefaultAzureCredential());

var idSecret = await secretClient.GetSecretAsync("client-id");
var clientSecret = await secretClient.GetSecretAsync("client-secret");
var refreshSecret = await secretClient.GetSecretAsync("refresh-token");
var mapsSecret = await secretClient.GetSecretAsync("MapsAPIKey");
var refreshAPIBaseSecret = await secretClient.GetSecretAsync("RefreshAPIBase");
var clientParamSecret = await secretClient.GetSecretAsync("ClientParam");
var clientSecretParamSecret = await secretClient.GetSecretAsync("ClientSecretParam");
var refreshTokenParamSecret = await secretClient.GetSecretAsync("RefreshTokenParam");
var refreshAPIEndSecret = await secretClient.GetSecretAsync("RefreshAPIEnd");

API api = new API();
api.client_id = idSecret.Value.Value;
api.client_secret = clientSecret.Value.Value;
api.refresh_token = refreshSecret.Value.Value;
api.maps_key = mapsSecret.Value.Value;
api.refresh_api_base = refreshAPIBaseSecret.Value.Value;
api.client_param = clientParamSecret.Value.Value;
api.client_secret_param = clientSecretParamSecret.Value.Value;
api.refresh_token_param = refreshTokenParamSecret.Value.Value;
api.refresh_api_end = refreshAPIEndSecret.Value.Value;

ActivitiesAPI.Instance(api);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=About}/{id?}");

app.Run();
