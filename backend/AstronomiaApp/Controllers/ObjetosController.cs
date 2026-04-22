using AstronomiaApp.Models.ViewModels;
using AstronomiaApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace AstronomiaApp.Controllers;

public class ObjetosController : Controller
{
    private readonly ObjetoService _service;

    public ObjetosController(ObjetoService service) => _service = service;

    // GET /Objetos  o  /
    public async Task<IActionResult> Index(string? tipo)
    {
        var objetos = await _service.ObtenerTodosAsync(tipo);
        var tipos = await _service.ObtenerTiposAsync();

        var vm = new CatalogoViewModel
        {
            Objetos = objetos.Select(o => new ObjetoViewModel
            {
                Id = o.Id,
                Nombre = o.Nombre,
                Tipo = o.Tipo?.Nombre ?? "",
                MasaKg = o.MasaKg,
                RadioKm = o.RadioKm,
                DistanciaTierraAl = o.DistanciaTierraAl,
                TemperaturaK = o.TemperaturaK,
                Sistema = o.Sistema?.Nombre,
            }),
            FiltroTipo = tipo,
            Total = objetos.Count()
        };

        ViewBag.Tipos = tipos;
        return View(vm);
    }

    // GET /Objetos/Detalle/{id}
    public async Task<IActionResult> Detalle(int id)
    {
        var obj = await _service.ObtenerPorIdAsync(id);
        if (obj == null) return NotFound();

        var vm = new ObjetoViewModel
        {
            Id = obj.Id,
            Nombre = obj.Nombre,
            Tipo = obj.Tipo?.Nombre ?? "",
            MasaKg = obj.MasaKg,
            RadioKm = obj.RadioKm,
            DistanciaTierraAl = obj.DistanciaTierraAl,
            TemperaturaK = obj.TemperaturaK,
            Luminosidad = obj.Luminosidad,
            Sistema = obj.Sistema?.Nombre,
            Constelacion = obj.Constelacion?.Nombre,
            Descripcion = obj.Descripcion
        };

        return View(vm);
    }

    // GET /Objetos/Buscar?nombre=...
    public async Task<IActionResult> Buscar(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            return RedirectToAction(nameof(Index));

        var (resultados, exacta) = await _service.BuscarPorNombreAsync(nombre);

        var vm = new BusquedaViewModel
        {
            Termino = nombre,
            Resultados = resultados.Select(o => new ObjetoViewModel
            {
                Id = o.Id,
                Nombre = o.Nombre,
                Tipo = o.Tipo?.Nombre ?? "",
                RadioKm = o.RadioKm,
                DistanciaTierraAl = o.DistanciaTierraAl,
                Sistema = o.Sistema?.Nombre,
            }),
            BusquedaExacta = exacta,
            TotalEncontrados = resultados.Count()
        };

        return View(vm);
    }

    // GET /Objetos/Ordenar?por=distancia&direccion=asc&tipo=Planeta
    public async Task<IActionResult> Ordenar(string por = "distancia", string direccion = "asc", string? tipo = null)
    {
        var objetos = await _service.OrdenarAsync(por, descendente: direccion == "desc", tipo: tipo);
        var tipos = await _service.ObtenerTiposAsync();

        var vm = new CatalogoViewModel
        {
            Objetos = objetos.Select(o => new ObjetoViewModel
            {
                Id = o.Id,
                Nombre = o.Nombre,
                Tipo = o.Tipo?.Nombre ?? "",
                MasaKg = o.MasaKg,
                RadioKm = o.RadioKm,
                DistanciaTierraAl = o.DistanciaTierraAl,
                TemperaturaK = o.TemperaturaK,
            }),
            OrdenPor = por,
            Direccion = direccion,
            FiltroTipo = tipo,
            Total = objetos.Count()
        };

        ViewBag.Tipos = tipos;
        return View("Index", vm);
    }
}
