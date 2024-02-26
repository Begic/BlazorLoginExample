using AspNetCore.Authentication.Basic;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services
    .AddAuthentication(BasicDefaults.AuthenticationScheme)
    .AddBasic<BasicUserValidationService>(options =>
    {
        options.Realm = "My App";
    }); // Service Registrieren 

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.UseAuthentication(); //Reihenfolge beachten
app.UseAuthorization();

app.MapRazorPages()
    .RequireAuthorization(); // ACHTUNG hier muss es aktiviert werden.

app.Run();

public class BasicUserValidationService : IBasicUserValidationService
{
    public Task<bool> IsValidAsync(string username, string password)
    {
        return Task.FromResult(password == "password"); // Hier Passwort abfangen, und kontrollieren.
    }
}
