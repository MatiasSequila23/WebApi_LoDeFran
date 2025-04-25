namespace WebApi_LoDeFran.ViewModels
{
    public class ClienteViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public int? PuntosFidelidad { get; set; }
        public DateTime? FechaCreacion { get; set; }
    }
}
