namespace WebApi_LoDeFran.ViewModels
{
    public class CategoriaProductoViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public List<SubcategoriaProductoViewModel> Subcategorias { get; set; } = new List<SubcategoriaProductoViewModel>();

    }
}
