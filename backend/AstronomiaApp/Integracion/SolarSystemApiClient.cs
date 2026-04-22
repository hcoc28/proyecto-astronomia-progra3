using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace AstronomiaApp.Integracion;

/// <summary>Cliente HTTP para Solar System OpenData API.</summary>
public class SolarSystemApiClient
{
    private const string BASE_URL = "https://api.le-systeme-solaire.net/rest";
    private readonly HttpClient _http;
    private readonly ILogger<SolarSystemApiClient> _logger;

    public SolarSystemApiClient(HttpClient http, ILogger<SolarSystemApiClient> logger)
    {
        _http = http;
        _logger = logger;
    }

    /// <summary>Obtiene todos los cuerpos celestes de la API.</summary>
    public async Task<List<CuerpoApiDto>> ObtenerTodosAsync()
    {
        try
        {
            var resp = await _http.GetFromJsonAsync<RespuestaBodies>($"{BASE_URL}/bodies");
            return resp?.Bodies ?? new List<CuerpoApiDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al consumir Solar System API");
            return new List<CuerpoApiDto>();
        }
    }

    /// <summary>Obtiene solo los planetas del Sistema Solar.</summary>
    public async Task<List<CuerpoApiDto>> ObtenerPlanetasAsync()
    {
        var todos = await ObtenerTodosAsync();
        return todos.Where(c => c.EsPlaneta).ToList();
    }

    /// <summary>Obtiene un cuerpo celeste por su ID de la API.</summary>
    public async Task<CuerpoApiDto?> ObtenerPorIdAsync(string apiId)
    {
        try
        {
            return await _http.GetFromJsonAsync<CuerpoApiDto>($"{BASE_URL}/bodies/{apiId}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo cuerpo {Id}", apiId);
            return null;
        }
    }
}

// ── DTOs de deserialización ─────────────────────────────────────────────

public class RespuestaBodies
{
    [JsonPropertyName("bodies")]
    public List<CuerpoApiDto> Bodies { get; set; } = new();
}

public class CuerpoApiDto
{
    [JsonPropertyName("id")]
    public string ApiId { get; set; } = "";

    [JsonPropertyName("name")]
    public string Nombre { get; set; } = "";

    [JsonPropertyName("englishName")]
    public string NombreIngles { get; set; } = "";

    [JsonPropertyName("isPlanet")]
    public bool EsPlaneta { get; set; }

    [JsonPropertyName("bodyType")]
    public string TipoCuerpo { get; set; } = "";

    [JsonPropertyName("mass")]
    public MasaDto? Masa { get; set; }

    [JsonPropertyName("meanRadius")]
    public double RadioMedio { get; set; }

    [JsonPropertyName("semimajorAxis")]
    public double SemiEjeMayor { get; set; }

    [JsonPropertyName("perihelion")]
    public double Perihelio { get; set; }

    [JsonPropertyName("aphelion")]
    public double Afelio { get; set; }

    [JsonPropertyName("avgTemp")]
    public double TemperaturaPromedio { get; set; }

    [JsonPropertyName("moons")]
    public List<LunaDto>? Lunas { get; set; }

    [JsonPropertyName("aroundPlanet")]
    public PlanetaRefDto? AlrededorDe { get; set; }

    public int CantidadLunas => Lunas?.Count ?? 0;

    /// <summary>Masa en kg calculada desde la notación científica de la API.</summary>
    public double? MasaKg => Masa != null ? Masa.MassValue * Math.Pow(10, Masa.MassExponent) : null;
}

public class MasaDto
{
    [JsonPropertyName("massValue")]
    public double MassValue { get; set; }

    [JsonPropertyName("massExponent")]
    public int MassExponent { get; set; }
}

public class LunaDto
{
    [JsonPropertyName("moon")]
    public string Nombre { get; set; } = "";

    [JsonPropertyName("rel")]
    public string Url { get; set; } = "";
}

public class PlanetaRefDto
{
    [JsonPropertyName("planet")]
    public string Nombre { get; set; } = "";

    [JsonPropertyName("rel")]
    public string Url { get; set; } = "";
}
