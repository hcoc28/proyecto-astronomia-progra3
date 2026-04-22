using AstronomiaApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace AstronomiaApp.Controllers.Api;

[ApiController]
[Route("api/admin")]
public class AdminApiController : ControllerBase
{
    private readonly ObjetoService _service;

    public AdminApiController(ObjetoService service) => _service = service;

    // POST /api/admin/importar
    [HttpPost("importar")]
    public async Task<IActionResult> Importar()
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();
        var (importados, actualizados, errores) = await _service.ImportarDesdeApiAsync();
        sw.Stop();

        return Ok(new
        {
            importados,
            actualizados,
            errores,
            duracionMs = (int)sw.ElapsedMilliseconds
        });
    }

    // GET /api/health
    [HttpGet("/api/health")]
    public IActionResult Health([FromServices] Data.AstronomiaDbContext db)
    {
        bool bdConectada;
        int totalObjetos = 0;
        try
        {
            bdConectada = db.Database.CanConnect();
            if (bdConectada) totalObjetos = db.ObjetosAstronomicos.Count();
        }
        catch { bdConectada = false; }

        return Ok(new
        {
            status = "ok",
            bd = bdConectada ? "conectada" : "sin conexión",
            objetosCargados = totalObjetos,
            timestamp = DateTime.UtcNow
        });
    }
}
