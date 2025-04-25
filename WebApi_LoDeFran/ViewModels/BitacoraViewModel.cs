namespace WebApi_LoDeFran.ViewModels
{
    public class BitacoraViewModel
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string? NombreUsuario { get; set; }
        public DateTime? Fecha { get; set; }
        public string Accion { get; set; } = null!;
        public string? Detalle { get; set; }
    }
}
