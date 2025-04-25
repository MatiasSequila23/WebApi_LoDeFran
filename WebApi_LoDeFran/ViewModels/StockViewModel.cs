namespace WebApi_LoDeFran.ViewModels
{
    public class StockViewModel
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public string ProductoNombre { get; set; } = null!;  // Nombre del producto
        public int CantidadDisponible { get; set; }
        public int CantidadEntrada { get; set; }
        public int CantidadSalida { get; set; }
        public DateTime? FechaActualizacion { get; set; }
    }
}
