using WebApi_LoDeFran.Models;

namespace WebApi_LoDeFran.ViewModels
{
    public class PedidoViewModel
    {
        public int Id { get; set; }
        public int? ClienteId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime? FechaPedido { get; set; }
        public decimal? Total { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public int EstadoId { get; set; }
        public int? CategoriaId { get; set; }
        public int? MesaId { get; set; }
        public int? TipoPedidoId { get; set; }
        public string? TipoPedidoNombre { get; set; }
        public EstadoInsumoViewModel Estado { get; set; }
        public List<DetallePedidoViewModel> DetallePedido { get; set; } = new();

        public MesaViewModel? Mesa { get; set; }
        public string? NombreMozo { get; set; }
        public string? Notas { get; set; }
        public string? ClienteNombre { get; set; }
        public List<PedidoComboViewModel> PedidoCombos { get; set; } = new();

        // 🔽 NUEVOS CAMPOS
        public int? PromocionId { get; set; }
        public string? NombrePromocion { get; set; } // opcional, para mostrar el nombre
        public decimal? MontoDescuento { get; set; }
        public decimal? TotalSinDescuento { get; set; }
    }


}
