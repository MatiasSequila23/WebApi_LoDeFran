//using WebApi_LoDeFran.Models;

namespace WebApi_LoDeFran.ViewModels
{
    public class DetallePedidoViewModel
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal? Subtotal { get; set; }
        public ProductoViewModel Producto { get; set; }
    }
}
