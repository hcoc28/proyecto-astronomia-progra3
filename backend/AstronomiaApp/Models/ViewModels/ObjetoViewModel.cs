namespace AstronomiaApp.Models.ViewModels;

public class ObjetoViewModel
{
    public int Id { get; set; }
    public string Nombre { get; set; } = "";
    public string Tipo { get; set; } = "";
    public double? MasaKg { get; set; }
    public double? RadioKm { get; set; }
    public double? DistanciaTierraAl { get; set; }
    public double? TemperaturaK { get; set; }
    public double? Luminosidad { get; set; }
    public string? Sistema { get; set; }
    public string? Constelacion { get; set; }
    public string? Descripcion { get; set; }
}

public class CatalogoViewModel
{
    public IEnumerable<ObjetoViewModel> Objetos { get; set; } = new List<ObjetoViewModel>();
    public string? FiltroTipo { get; set; }
    public string? OrdenPor { get; set; }
    public string Direccion { get; set; } = "asc";
    public int Total { get; set; }
}

public class BusquedaViewModel
{
    public string Termino { get; set; } = "";
    public IEnumerable<ObjetoViewModel> Resultados { get; set; } = new List<ObjetoViewModel>();
    public int TotalEncontrados { get; set; }
    public bool BusquedaExacta { get; set; }
}

public class GrafoViewModel
{
    public IEnumerable<NodoGrafoViewModel> Nodos { get; set; } = new List<NodoGrafoViewModel>();
    public IEnumerable<AristaGrafoViewModel> Aristas { get; set; } = new List<AristaGrafoViewModel>();
}

public class NodoGrafoViewModel
{
    public int Id { get; set; }
    public string Nombre { get; set; } = "";
    public string Tipo { get; set; } = "";
}

public class AristaGrafoViewModel
{
    public int Origen { get; set; }
    public int Destino { get; set; }
    public string TipoRelacion { get; set; } = "";
    public double Peso { get; set; }
}
