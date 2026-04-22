using AstronomiaApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace AstronomiaApp.Controllers;

public class GrafoController : Controller
{
    private readonly GrafoService _grafoService;

    public GrafoController(GrafoService grafoService) => _grafoService = grafoService;

    // GET /Grafo
    public async Task<IActionResult> Index()
    {
        var vm = await _grafoService.ObtenerGrafoCompletoAsync();
        return View(vm);
    }
}
