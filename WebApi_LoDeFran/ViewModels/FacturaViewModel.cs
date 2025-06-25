namespace WebApi_LoDeFran.ViewModels
{
    public class FacturaViewModel
    {
        public int Id { get; set; }

        public int PedidoId { get; set; }

        public decimal Total { get; set; }

        public DateTime? FechaEmision { get; set; }

        public string Estado { get; set; } = null!;

        public int? ClienteId { get; set; }

        public string? ClienteNombre { get; set; }  // Para mostrar "Juan Pérez"

        public int MetodoPagoId { get; set; }

        public string? MetodoPagoNombre { get; set; }
    }
}
