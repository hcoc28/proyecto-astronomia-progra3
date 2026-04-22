using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AstronomiaApp.Models;

[Table("tipos_objeto")]
public class TipoObjeto
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    [Column("nombre")]
    public string Nombre { get; set; } = "";

    [Column("descripcion")]
    public string? Descripcion { get; set; }

    public ICollection<ObjetoAstronomico> Objetos { get; set; } = new List<ObjetoAstronomico>();
}
