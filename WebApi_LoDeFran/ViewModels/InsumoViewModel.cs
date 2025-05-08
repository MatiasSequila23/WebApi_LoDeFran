using System;

namespace WebApi_LoDeFran.ViewModels
{
    public class InsumoViewModel
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        public string? UnidadMedida { get; set; }

        public decimal? Costo { get; set; }

        public decimal CantidadDisponible { get; set; }

        public int? ProveedorId { get; set; }
        public string? ProveedorNombre { get; set; }

        public DateTime? FechaUltimoIngreso { get; set; }

        public decimal CantidadMinima { get; set; }
        public int? EstadoId { get; set; }
        public string? EstadoNombre { get; set; }
    }
}
