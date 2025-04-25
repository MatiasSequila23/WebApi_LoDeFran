namespace WebApi_LoDeFran.ViewModels
{
    public class RolViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public List<string> Permisos { get; set; } = new List<string>();
    }
}
