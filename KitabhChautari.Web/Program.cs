using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using KitabhChautari.Web;
using Refit;
using KitabhChautari.Web.Apis;
using static System.Net.WebRequestMethods;
using KitabhChautari.Web.Auth;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<KitabhChautariAuthStateProvider>();
builder.Services.AddSingleton<AuthenticationStateProvider>(sp => sp.GetRequiredService<KitabhChautariAuthStateProvider>());
builder.Services.AddAuthorizationCore();


ConfigureRefit(builder.Services);


await builder.Build().RunAsync();

static void ConfigureRefit(IServiceCollection services)
{
    const string ApiBaseUrl = "https://localhost:7127";
    services.AddRefitClient < IAuthApi>()
        .ConfigureHttpClient(httpClient => httpClient.BaseAddress = new Uri(ApiBaseUrl));
}
