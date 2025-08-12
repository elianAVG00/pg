using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SPSCierreLote.EFCore.models;
using SPSCierreLote.Models;

namespace SPSCierreLote.Repositories;

/// <summary>
/// Lectura de parámetros globales (tabla <c>AppConfig</c>) y de la vista
/// <c>GetPrismaCodes</c> para cachear la relación <em>IDSite ↔ Product</em>.
/// </summary>
public static class AppConfigRepository
{
    // ==========================================================
    // 1) SITIOS / PRODUCTOS (vista GetPrismaCodes) ─────────────
    // ==========================================================
    public static List<SiteProduct> GetSitios()
    {
        try
        {
            using var ctx = Program.ServiceProvider.GetRequiredService<PgDbContext>();

            // La vista GetPrismaCodes expone: ProductId, IDSITE, SPSPrisma, SPSNormal
            var sitios = ctx.GetPrismaCodes
                            .AsNoTracking()
                            .Select(g => new SiteProduct
                            {
                                ProductId = g.ProductId,
                                IDSite = g.UniqueCode,
                                SPSPrisma = g.Prisma,
                                SPSNormal = g.Normal
                            })
                            .ToList();

            return sitios;
        }
        catch (Exception ex)
        {
            Program.Print("ERROR AL LEER LA CONFIGURACIÓN DE SITIOS", "ERROR");
            Program.Logger.LogError(ex, "Error F02.1");
            return new List<SiteProduct>();
        }
    }

    // ==========================================================
    // 2) VALOR ÚNICO DE AppConfig ──────────────────────────────
    // ==========================================================
    public static string GetConfiguration(string setting)
    {
        try
        {
            using var ctx = Program.ServiceProvider.GetRequiredService<PgDbContext>();

            string? value = ctx.AppConfig
                               .AsNoTracking()
                               .Where(c => c.Setting == setting)
                               .Select(c => c.Value)
                               .FirstOrDefault();

            if (value is null)
            {
                Program.Logger.LogWarning("AppConfig «{setting}» no existe en DB", setting);
                return string.Empty;
            }

            Program.Logger.LogInformation("AppConfig «{setting}» = {value}", setting, value);
            return value ?? string.Empty;
        }
        catch (Exception ex)
        {
            Program.Logger.LogError(ex, "Error F02 leyendo AppConfig {setting}", setting);
            return string.Empty;
        }
    }
}
