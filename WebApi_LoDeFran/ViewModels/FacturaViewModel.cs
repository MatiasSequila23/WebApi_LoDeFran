namespace WebApi_LoDeFran.ViewModels
{
    public class FacturaViewModel
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public decimal Total { get; set; }
        public string? MetodoPago { get; set; }
        public DateTime? FechaEmision { get; set; }
        public string Estado { get; set; } = null!;
    }
}
