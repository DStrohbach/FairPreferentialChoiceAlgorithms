using FairPreferentialChoiceAlgorithms.Components;
using FairPreferentialChoiceAlgorithms.Services;
using MudBlazor.Services;

namespace FairPreferentialChoiceAlgorithms
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. Konfiguration laden
            // - Lese Random-Seed-Wert aus appsettings.json
            int? seed = builder.Configuration.GetValue<int?>("Settings:RandomSeed");
            // - Registriere Random (mit der Seed) im DependenyInjection-Container.
            // - Jeder Service, der Random erwartet, bekommt ihn nun vom Framework injiziert.
            builder.Services.AddSingleton<Random>(_ =>
                seed.HasValue ? new Random(seed.Value) : new Random()
            );

            // 2. Dienste registrieren
            builder.Services.AddSingleton<InputDataService>();  // Testdaten erstellen
            builder.Services.AddSingleton<AssignmentService>(); // Zuteilungen erstellen
            builder.Services.AddSingleton<MetricsService>();    // Metriken erstellen

            // 3. Razor-Komponenten aktivieren (für Blazor Server)
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();
            builder.Services.AddBlazorBootstrap();  // Frontend-Komponenten
            builder.Services.AddMudServices();
            var app = builder.Build();

            // 4. Middleware-Pipeline konfigurieren
            // Bildlich: Browser -> Middleware 1 -> ... -> Middleware n -> Razor Page
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");  // Fehlerseite für Produktion
                app.UseHsts();                      // HTTP Strict Transport Security
            }
          
            app.UseHttpsRedirection();              // HTTPS-Umleitung: Erzwingt sichere Verbindungen
            app.UseAntiforgery();                   // Antiforgery: Schutz vor Cross-Site-Angriffen
            app.MapStaticAssets();                  // Static Files: Liefert Bilder, JS, CSS direkt aus
            app.MapRazorComponents<App>()           // Blazor-Hauptkomponente: Verarbeitet .razor-Seiten
                .AddInteractiveServerRenderMode();  // Rendermodus global auf InteractiveServer setzen, ermöglich JS

            // 5. Anwendung starten
            app.Run();
        }
    }
}