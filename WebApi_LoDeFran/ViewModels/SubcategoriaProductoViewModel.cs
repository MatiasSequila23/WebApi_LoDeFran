namespace WebApi_LoDeFran.ViewModels
{
    public class SubcategoriaProductoViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;

        // Incluimos la categoría de referencia
        public int CategoriaId { get; set; }
        public string CategoriaNombre { get; set; } = string.Empty;

        // Lista de productos si querés devolverlos
        public List<ProductoViewModel> Productos { get; set; } = new();

    }
}
