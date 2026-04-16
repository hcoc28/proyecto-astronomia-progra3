// =====================================================================
// Prototipo de consumo de API externa - Fase 1
// Proyecto Astronomia - Programacion III
//
// Consume Solar System OpenData API y muestra los primeros planetas
// en consola. Sirve como prueba funcional para la Primera Revision.
// =====================================================================

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PrototipoApi;

public class Program
{
    private const string API_BASE = "https://api.le-systeme-solaire.net/rest";

    public static async Task Main(string[] args)
    {
        Console.WriteLine("========================================================");
        Console.WriteLine("  Prototipo de consumo - Solar System OpenData API");
        Console.WriteLine("  Proyecto Astronomia - Programacion III");
        Console.WriteLine("========================================================\n");

        using var http = new HttpClient();
        http.Timeout = TimeSpan.FromSeconds(30);

        try
        {
            // Consumo del endpoint /bodies - devuelve todos los cuerpos celestes
            var url = $"{API_BASE}/bodies";
            Console.WriteLine($"GET {url}\n");

            var respuesta = await http.GetFromJsonAsync<RespuestaBodies>(url);

            if (respuesta == null || respuesta.Bodies == null)
            {
                Console.WriteLine("La API no devolvio datos.");
                return;
            }

            Console.WriteLine($"Total de cuerpos celestes recibidos: {respuesta.Bodies.Count}\n");

            // Filtrar solo planetas y mostrarlos ordenados por distancia al Sol
            var planetas = new List<Cuerpo>();
            foreach (var c in respuesta.Bodies)
            {
                if (c.IsPlanet)
                {
                    planetas.Add(c);
                }
            }

            // Ordenamiento simple (burbuja) por semi-eje mayor (distancia al Sol)
            for (int i = 0; i < planetas.Count - 1; i++)
            {
                for (int j = 0; j < planetas.Count - i - 1; j++)
                {
                    if (planetas[j].SemiMajorAxis > planetas[j + 1].SemiMajorAxis)
                    {
                        (planetas[j], planetas[j + 1]) = (planetas[j + 1], planetas[j]);
                    }
                }
            }

            Console.WriteLine("PLANETAS DEL SISTEMA SOLAR (ordenados por distancia al Sol):");
            Console.WriteLine("----------------------------------------------------------------");
            Console.WriteLine($"{"Nombre",-15} {"Radio (km)",-15} {"Distancia (km)",-20} {"Lunas",-10}");
            Console.WriteLine("----------------------------------------------------------------");

            foreach (var p in planetas)
            {
                int lunas = p.Moons != null ? p.Moons.Count : 0;
                Console.WriteLine($"{p.EnglishName,-15} {p.MeanRadius,-15:N0} {p.SemiMajorAxis,-20:N0} {lunas,-10}");
            }

            Console.WriteLine("\nPrueba completada con exito.");
            Console.WriteLine("La API responde correctamente y los datos son deserializables.\n");
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error de red: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error inesperado: {ex.Message}");
        }
    }
}

// ---------------------------------------------------------------------
// Modelos de deserializacion del JSON de la API
// ---------------------------------------------------------------------

public class RespuestaBodies
{
    [JsonPropertyName("bodies")]
    public List<Cuerpo> Bodies { get; set; } = new();
}

public class Cuerpo
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = "";

    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("englishName")]
    public string EnglishName { get; set; } = "";

    [JsonPropertyName("isPlanet")]
    public bool IsPlanet { get; set; }

    [JsonPropertyName("meanRadius")]
    public double MeanRadius { get; set; }

    [JsonPropertyName("semimajorAxis")]
    public double SemiMajorAxis { get; set; }

    [JsonPropertyName("moons")]
    public List<Luna>? Moons { get; set; }
}

public class Luna
{
    [JsonPropertyName("moon")]
    public string Moon { get; set; } = "";
}
