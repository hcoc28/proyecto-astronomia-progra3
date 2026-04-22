using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AstronomiaApp.Models;

[Table("constelaciones")]
public class Constelacion
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("nombre")]
    public string Nombre { get; set; } = "";

    [MaxLength(3)]
    [Column("abreviatura")]
    public string? Abreviatura { get; set; }

    [Column("descripcion")]
    public string? Descripcion { get; set; }

    public ICollection<ObjetoAstronomico> Objetos { get; set; } = new List<ObjetoAstronomico>();
}
