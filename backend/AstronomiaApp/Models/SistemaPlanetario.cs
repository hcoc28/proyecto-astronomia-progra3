using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AstronomiaApp.Models;

[Table("sistemas_planetarios")]
public class SistemaPlanetario
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    [Column("nombre")]
    public string Nombre { get; set; } = "";

    [Column("estrella_central_id")]
    public int? EstrellaCentralId { get; set; }

    [Column("descripcion")]
    public string? Descripcion { get; set; }

    public ICollection<ObjetoAstronomico> Objetos { get; set; } = new List<ObjetoAstronomico>();
}
