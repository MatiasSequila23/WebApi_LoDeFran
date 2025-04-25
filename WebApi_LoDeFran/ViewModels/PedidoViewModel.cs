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
        public EstadosPedido Estado { get; set; }
        public List<DetallePedidoViewModel> DetallePedido { get; set; } = new List<DetallePedidoViewModel>();
    }
}
