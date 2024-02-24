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
                // Hier können Sie Ihre eigene Logik zur Überprüfung der Anmeldeinformationen implementieren
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

app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapRazorPages();

app.Run();
