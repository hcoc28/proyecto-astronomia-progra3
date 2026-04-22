using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AstronomiaApp.Models;

[Table("consultas_log")]
public class ConsultaLog
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [MaxLength(50)]
    [Column("tipo_consulta")]
    public string? TipoConsulta { get; set; }

    [Column("parametros")]
    public string? Parametros { get; set; }

    [Column("resultado_count")]
    public int? ResultadoCount { get; set; }

    [Column("duracion_ms")]
    public int? DuracionMs { get; set; }

    [Column("fecha")]
    public DateTime Fecha { get; set; } = DateTime.Now;
}
