using AstronomiaApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace AstronomiaApp.Controllers.Api;

[ApiController]
[Route("api/objetos")]
public class ObjetosApiController : ControllerBase
{
    private readonly ObjetoService _service;

    public ObjetosApiController(ObjetoService service) => _service = service;

    // GET /api/objetos?tipo=Planeta&limit=100&offset=0
    [HttpGet]
    public async Task<IActionResult> Listar(string? tipo, int limit = 100, int offset = 0)
    {
        var todos = await _service.ObtenerTodosAsync(tipo);
        var paginados = todos.Skip(offset).Take(limit);

        return Ok(new
        {
            total = todos.Count(),
            items = paginados.Select(o => new
            {
                id = o.Id,
                nombre = o.Nombre,
                tipo = o.Tipo?.Nombre,
                masa = o.MasaKg,
                radio = o.RadioKm,
                distanciaTierraAl = o.DistanciaTierraAl,
                temperatura = o.TemperaturaK,
                sistema = o.Sistema?.Nombre
            })
        });
    }

    // GET /api/objetos/{id}
    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var obj = await _service.ObtenerPorIdAsync(id);
        if (obj == null) return NotFound(new { error = true, mensaje = "Objeto no encontrado", codigo = "NOT_FOUND" });

        return Ok(new
        {
            id = obj.Id,
            nombre = obj.Nombre,
            tipo = obj.Tipo?.Nombre,
            masa = obj.MasaKg,
            radio = obj.RadioKm,
            distanciaTierraAl = obj.DistanciaTierraAl,
            temperatura = obj.TemperaturaK,
            luminosidad = obj.Luminosidad,
            sistema = obj.Sistema?.Nombre,
            constelacion = obj.Constelacion?.Nombre,
            descripcion = obj.Descripcion
        });
    }

    // GET /api/objetos/buscar?nombre=Marte
    [HttpGet("buscar")]
    public async Task<IActionResult> Buscar(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            return BadRequest(new { error = true, mensaje = "Parámetro 'nombre' requerido" });

        var (resultados, exacta) = await _service.BuscarPorNombreAsync(nombre);
        return Ok(new
        {
            encontrados = resultados.Count(),
            busquedaExacta = exacta,
            items = resultados.Select(o => new { id = o.Id, nombre = o.Nombre, tipo = o.Tipo?.Nombre })
        });
    }

    // GET /api/objetos/ordenar?por=distancia&direccion=asc&tipo=Planeta
    [HttpGet("ordenar")]
    public async Task<IActionResult> Ordenar(string por = "distancia", string direccion = "asc", string? tipo = null)
    {
        var ordenados = await _service.OrdenarAsync(por, descendente: direccion == "desc", tipo: tipo);
        return Ok(new
        {
            total = ordenados.Count(),
            ordenadoPor = por,
            direccion,
            items = ordenados.Select(o => new
            {
                id = o.Id,
                nombre = o.Nombre,
                tipo = o.Tipo?.Nombre,
                masa = o.MasaKg,
                radio = o.RadioKm,
                distanciaTierraAl = o.DistanciaTierraAl,
                temperatura = o.TemperaturaK
            })
        });
    }

    // GET /api/objetos/rango?campo=distancia&min=0&max=500
    [HttpGet("rango")]
    public async Task<IActionResult> BuscarRango(string campo, double min, double max)
    {
        var resultados = await _service.BuscarRangoAsync(campo, min, max);
        return Ok(new
        {
            campo,
            min,
            max,
            total = resultados.Count(),
            items = resultados.Select(o => new { id = o.Id, nombre = o.Nombre, tipo = o.Tipo?.Nombre })
        });
    }
}
