using ERPManagement.Application.Contracts.Infrastructure.Services;
using ERPManagement.UI;
using ERPManagement.UI.DataModels;
using ERPManagement.UI.DataModels.Accounting.MasterData.Currency;
using ERPManagement.UI.GeneralClasses;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddAllGenericServices();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<WebsiteAuthenticator>();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddScoped<AuthenticationStateProvider, WebsiteAuthenticator>();
builder.Services.AddSignalR(hubOptions =>
{
    hubOptions.MaximumReceiveMessageSize = 10 * 1024 * 1024;
});

#region Validation Container
builder.Services.AddScoped<IValidator<Currency>, CurrencyValidator>();
#endregion
//Localization Container
builder.Services.AddLocalization();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    // You can set the default language using the following method:
    options.SetDefaultCulture("en");
    options.AddSupportedCultures(new[] { "en", "ar" });
    options.AddSupportedUICultures(new[] { "en", "ar" });
});

var app = await builder
    .ConfigureServices()
    .ConfigurePipeline();

//Comment by shaimaa
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

//Add Request Localization to your Request Pipeline
app.UseRequestLocalization();
app.MapGet("Culture/Set", (string uiculture, string redirectUri, HttpContext context) =>
{
    if (uiculture is not null)
    {
        var options = new CookieOptions
        {
            Expires = DateTime.Now.AddYears(1),
            IsEssential = true
        };
        context.Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture("en", uiculture)), options);
    }

    return Results.LocalRedirect(redirectUri);
});

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
