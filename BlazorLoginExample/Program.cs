using AspNetCore.Authentication.Basic;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddAuthentication(BasicDefaults.AuthenticationScheme)
    .AddBasic(options =>
    {
        options.Realm = "My Realm";
        options.Events = new BasicEvents()
        {
            OnValidateCredentials = context =>
            {
                // Hier k�nnen Sie Ihre eigene Logik zur �berpr�fung der Anmeldeinformationen implementieren
                if (context.Username == "username" && context.Password == "password")
                {
                    context.Principal = new System.Security.Claims.ClaimsPrincipal();
                    context.Success();
                }
                else
                {
                    context.NoResult();
                }

                return Task.CompletedTask;
            }
        };
    });

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

app.UseAuthorization();
app.UseAuthentication();
app.MapRazorPages().RequireAuthorization();

app.Run();
