namespace WebApi_LoDeFran.ViewModels
{
    public class UsuarioViewModel
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public string Email { get; set; } = null!;

        public DateTime? FechaCreacion { get; set; }

        public List<string> Roles { get; set; } = new();

        public List<string> Permisos { get; set; } = new();
    }
}
