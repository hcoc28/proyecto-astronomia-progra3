using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AstronomiaApp.Models;

[Table("objetos_astronomicos")]
public class ObjetoAstronomico
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("tipo_id")]
    public int TipoId { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("nombre")]
    public string Nombre { get; set; } = "";

    [Column("masa_kg")]
    public double? MasaKg { get; set; }

    [Column("radio_km")]
    public double? RadioKm { get; set; }

    [Column("distancia_tierra_al")]
    public double? DistanciaTierraAl { get; set; }

    [Column("temperatura_k")]
    public double? TemperaturaK { get; set; }

    [Column("luminosidad")]
    public double? Luminosidad { get; set; }

    [Column("sistema_id")]
    public int? SistemaId { get; set; }

    [Column("constelacion_id")]
    public int? ConstelacionId { get; set; }

    [Column("descripcion")]
    public string? Descripcion { get; set; }

    [Column("fecha_creacion")]
    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    // Navegacion
    public TipoObjeto? Tipo { get; set; }
    public SistemaPlanetario? Sistema { get; set; }
    public Constelacion? Constelacion { get; set; }

    public ICollection<Relacion> RelacionesOrigen { get; set; } = new List<Relacion>();
    public ICollection<Relacion> RelacionesDestino { get; set; } = new List<Relacion>();
}
