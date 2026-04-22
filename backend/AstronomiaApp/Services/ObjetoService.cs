using AstronomiaApp.Data;
using AstronomiaApp.EstructurasDatos;
using AstronomiaApp.Integracion;
using AstronomiaApp.Models;
using AstronomiaApp.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AstronomiaApp.Services;

/// <summary>Servicio principal — coordina BD, estructuras de datos y API externa.</summary>
public class ObjetoService
{
    private readonly AstronomiaDbContext _db;
    private readonly SolarSystemApiClient _apiClient;
    private readonly ILogger<ObjetoService> _logger;

    // Estructuras en memoria (se cargan al iniciar la app)
    private readonly ListaEnlazada<ObjetoAstronomico> _lista = new();
    private TablaHash<ObjetoAstronomico> _hash = new();
    private bool _cargado = false;

    public ObjetoService(AstronomiaDbContext db, SolarSystemApiClient apiClient, ILogger<ObjetoService> logger)
    {
        _db = db;
        _apiClient = apiClient;
        _logger = logger;
    }

    // ── Carga en memoria ─────────────────────────────────────────────────

    /// <summary>Carga todos los objetos de BD en lista enlazada y tabla hash.</summary>
    public async Task CargarEstructurasAsync()
    {
        if (_cargado) return;

        _lista.Limpiar();
        _hash = new TablaHash<ObjetoAstronomico>();

        var objetos = await _db.ObjetosAstronomicos
            .Include(o => o.Tipo)
            .Include(o => o.Sistema)
            .Include(o => o.Constelacion)
            .ToListAsync();

        foreach (var obj in objetos)
        {
            _lista.AgregarAlFinal(obj);
            _hash.Insertar(obj.Nombre, obj);
        }

        _cargado = true;
        _logger.LogInformation("Estructuras cargadas: {Count} objetos", objetos.Count);
    }

    public void InvalidarCache() => _cargado = false;

    // ── Catálogo ─────────────────────────────────────────────────────────

    /// <summary>Obtiene catálogo completo desde lista enlazada.</summary>
    public async Task<IEnumerable<ObjetoAstronomico>> ObtenerTodosAsync(string? tipo = null)
    {
        await CargarEstructurasAsync();
        if (tipo == null) return _lista.ObtenerTodos();
        return _lista.Filtrar(o => o.Tipo?.Nombre.Equals(tipo, StringComparison.OrdinalIgnoreCase) == true);
    }

    public async Task<ObjetoAstronomico?> ObtenerPorIdAsync(int id)
    {
        return await _db.ObjetosAstronomicos
            .Include(o => o.Tipo)
            .Include(o => o.Sistema)
            .Include(o => o.Constelacion)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    // ── Búsqueda ─────────────────────────────────────────────────────────

    /// <summary>
    /// Busca por nombre. Primero tabla hash (exacta O(1)),
    /// luego búsqueda parcial en lista si no encuentra.
    /// </summary>
    public async Task<(IEnumerable<ObjetoAstronomico> Resultados, bool ExactaEncontrada)> BuscarPorNombreAsync(string nombre)
    {
        await CargarEstructurasAsync();

        // 1. Búsqueda exacta con tabla hash
        if (_hash.Buscar(nombre, out var exacto) && exacto != null)
            return (new[] { exacto }, true);

        // 2. Búsqueda parcial en lista enlazada
        var parciales = _lista.Filtrar(o =>
            o.Nombre.Contains(nombre, StringComparison.OrdinalIgnoreCase)).ToList();

        return (parciales, false);
    }

    // ── Ordenamiento ─────────────────────────────────────────────────────

    /// <summary>Ordena usando árbol AVL. O(n log n) inserción + O(n) recorrido.</summary>
    public async Task<IEnumerable<ObjetoAstronomico>> OrdenarAsync(string campo, bool descendente = false, string? tipo = null)
    {
        await CargarEstructurasAsync();

        var avl = new ArbolAVL<ObjetoAstronomico>();
        var fuente = tipo != null
            ? _lista.Filtrar(o => o.Tipo?.Nombre.Equals(tipo, StringComparison.OrdinalIgnoreCase) == true)
            : _lista.ObtenerTodos();

        foreach (var obj in fuente)
        {
            double clave = campo.ToLower() switch
            {
                "masa"        => obj.MasaKg ?? 0,
                "radio"       => obj.RadioKm ?? 0,
                "distancia"   => obj.DistanciaTierraAl ?? 0,
                "temperatura" => obj.TemperaturaK ?? 0,
                "luminosidad" => obj.Luminosidad ?? 0,
                _             => obj.Id
            };
            avl.Insertar(clave, obj);
        }

        return descendente ? avl.InordenDescendente() : avl.Inorden();
    }

    /// <summary>Busca objetos en rango usando árbol AVL.</summary>
    public async Task<IEnumerable<ObjetoAstronomico>> BuscarRangoAsync(string campo, double min, double max)
    {
        await CargarEstructurasAsync();

        var avl = new ArbolAVL<ObjetoAstronomico>();
        foreach (var obj in _lista.ObtenerTodos())
        {
            double clave = campo.ToLower() switch
            {
                "masa"        => obj.MasaKg ?? 0,
                "radio"       => obj.RadioKm ?? 0,
                "distancia"   => obj.DistanciaTierraAl ?? 0,
                "temperatura" => obj.TemperaturaK ?? 0,
                _             => obj.Id
            };
            avl.Insertar(clave, obj);
        }

        return avl.BuscarRango(min, max);
    }

    // ── Importación desde API ─────────────────────────────────────────────

    /// <summary>Importa planetas desde Solar System API y los guarda en BD.</summary>
    public async Task<(int Importados, int Actualizados, int Errores)> ImportarDesdeApiAsync()
    {
        int importados = 0, actualizados = 0, errores = 0;

        try
        {
            var cuerpos = await _apiClient.ObtenerTodosAsync();

            // Asegurar que existe tipo "Planeta"
            var tipoPlaneta = await _db.TiposObjeto.FirstOrDefaultAsync(t => t.Nombre == "Planeta")
                              ?? new TipoObjeto { Nombre = "Planeta", Descripcion = "Cuerpo celeste que orbita una estrella" };
            if (tipoPlaneta.Id == 0) { _db.TiposObjeto.Add(tipoPlaneta); await _db.SaveChangesAsync(); }

            var tipoSatelite = await _db.TiposObjeto.FirstOrDefaultAsync(t => t.Nombre == "Satélite")
                               ?? new TipoObjeto { Nombre = "Satélite", Descripcion = "Luna o cuerpo en órbita de un planeta" };
            if (tipoSatelite.Id == 0) { _db.TiposObjeto.Add(tipoSatelite); await _db.SaveChangesAsync(); }

            // Asegurar sistema Solar
            var sistemaSolar = await _db.SistemasPlanetarios.FirstOrDefaultAsync(s => s.Nombre == "Sistema Solar")
                               ?? new SistemaPlanetario { Nombre = "Sistema Solar" };
            if (sistemaSolar.Id == 0) { _db.SistemasPlanetarios.Add(sistemaSolar); await _db.SaveChangesAsync(); }

            foreach (var cuerpo in cuerpos)
            {
                try
                {
                    bool esPlaneta = cuerpo.EsPlaneta;
                    bool esSatelite = cuerpo.AlrededorDe != null;
                    if (!esPlaneta && !esSatelite) continue; // solo planetas y lunas

                    var tipo = esPlaneta ? tipoPlaneta : tipoSatelite;
                    var existente = await _db.ObjetosAstronomicos
                        .FirstOrDefaultAsync(o => o.Nombre == cuerpo.NombreIngles || o.Nombre == cuerpo.Nombre);

                    if (existente != null)
                    {
                        existente.MasaKg = cuerpo.MasaKg;
                        existente.RadioKm = cuerpo.RadioMedio;
                        existente.DistanciaTierraAl = cuerpo.SemiEjeMayor > 0 ? cuerpo.SemiEjeMayor : null;
                        existente.TemperaturaK = cuerpo.TemperaturaPromedio > 0 ? cuerpo.TemperaturaPromedio : null;
                        actualizados++;
                    }
                    else
                    {
                        var nuevo = new ObjetoAstronomico
                        {
                            Nombre = string.IsNullOrWhiteSpace(cuerpo.NombreIngles) ? cuerpo.Nombre : cuerpo.NombreIngles,
                            TipoId = tipo.Id,
                            MasaKg = cuerpo.MasaKg,
                            RadioKm = cuerpo.RadioMedio > 0 ? cuerpo.RadioMedio : null,
                            DistanciaTierraAl = cuerpo.SemiEjeMayor > 0 ? cuerpo.SemiEjeMayor : null,
                            TemperaturaK = cuerpo.TemperaturaPromedio > 0 ? cuerpo.TemperaturaPromedio : null,
                            SistemaId = esPlaneta ? sistemaSolar.Id : null,
                        };
                        _db.ObjetosAstronomicos.Add(nuevo);
                        importados++;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Error importando cuerpo {Nombre}", cuerpo.Nombre);
                    errores++;
                }
            }

            await _db.SaveChangesAsync();
            InvalidarCache();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error general en importación");
            errores++;
        }

        return (importados, actualizados, errores);
    }

    // ── Tipos ─────────────────────────────────────────────────────────────

    public async Task<IEnumerable<TipoObjeto>> ObtenerTiposAsync() =>
        await _db.TiposObjeto.ToListAsync();
}
