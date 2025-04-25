namespace WebApi_LoDeFran.ViewModels
{
    public class ProductoViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public DateTime? FechaCreacion { get; set; }

        public int? CategoriaProductoId { get; set; }
        public string? CategoriaProductoNombre { get; set; } // Para mostrar el nombre

        public int? EstadoId { get; set; }
        public string? EstadoNombre { get; set; }
    }
}
