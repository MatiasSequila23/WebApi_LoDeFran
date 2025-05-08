namespace WebApi_LoDeFran.ViewModels
{
    public class InsumoProductoViewModel
    {
        public int ProductoId { get; set; }
        public int InsumoId { get; set; }
        public string? NombreInsumo { get; set; } // opcional, para mostrar en el frontend
        public decimal? Cantidad { get; set; }
    }
}
