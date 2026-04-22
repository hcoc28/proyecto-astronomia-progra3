using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AstronomiaApp.Models;

[Table("relaciones")]
public class Relacion
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("origen_id")]
    public int OrigenId { get; set; }

    [Required]
    [Column("destino_id")]
    public int DestinoId { get; set; }

    [MaxLength(50)]
    [Column("tipo_relacion")]
    public string? TipoRelacion { get; set; }

    [Column("distancia_al")]
    public double? DistanciaAl { get; set; }

    // Navegacion — sin cascade (SQL Server no permite multiples cascadas desde misma tabla)
    public ObjetoAstronomico? Origen { get; set; }
    public ObjetoAstronomico? Destino { get; set; }
}
