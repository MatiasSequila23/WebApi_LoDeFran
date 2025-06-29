namespace WebApi_LoDeFran.ViewModels
{
    public class MotivoMovimientoViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public int TipoMovimientoId { get; set; }
        public string? TipoMovimientoNombre { get; set; }  // Para mostrar en el frontend
    }
}
