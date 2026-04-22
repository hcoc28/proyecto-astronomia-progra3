using AstronomiaApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace AstronomiaApp.Controllers.Api;

[ApiController]
[Route("api/grafo")]
public class GrafoApiController : ControllerBase
{
    private readonly GrafoService _grafoService;

    public GrafoApiController(GrafoService grafoService) => _grafoService = grafoService;

    // GET /api/grafo/vecinos/{id}
    [HttpGet("vecinos/{id:int}")]
    public async Task<IActionResult> ObtenerVecinos(int id)
    {
        var vecinos = await _grafoService.ObtenerVecinosAsync(id);
        return Ok(new
        {
            origenId = id,
            vecinos = vecinos.Select(v => new
            {
                id = v.Id,
                nombre = v.Nombre,
                tipoRelacion = v.TipoRelacion,
                distanciaAl = v.Peso
            })
        });
    }

    // GET /api/grafo/ruta?origen=1&destino=30
    [HttpGet("ruta")]
    public async Task<IActionResult> CalcularRuta(int origen, int destino)
    {
        var resultado = await _grafoService.CalcularRutaAsync(origen, destino);

        if (!resultado.Encontrada)
            return NotFound(new { error = true, mensaje = "No existe ruta entre los objetos indicados", codigo = "NO_ROUTE" });

        return Ok(new
        {
            origen,
            destino,
            distanciaTotal = resultado.DistanciaTotal,
            saltos = resultado.Ruta.Count - 1,
            ruta = resultado.Ruta
        });
    }

    // GET /api/grafo/sistema/{idSistema}
    [HttpGet("sistema/{idSistema:int}")]
    public async Task<IActionResult> ExplorarSistema(int idSistema)
    {
        var nodos = await _grafoService.BFSSistemaAsync(idSistema);
        return Ok(new
        {
            sistemaId = idSistema,
            nodos = nodos.ToList()
        });
    }
}
