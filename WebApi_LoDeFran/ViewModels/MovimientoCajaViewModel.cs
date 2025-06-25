namespace WebApi_LoDeFran.ViewModels
{
    public class MovimientoCajaViewModel
    {
        public int Id { get; set; }

        public int CajaId { get; set; }

        public DateTime FechaMovimiento { get; set; }

        public string TipoMovimiento { get; set; } = null!; // Ej: "Ingreso", "Egreso"

        public decimal Monto { get; set; }

        public string? Descripcion { get; set; }

        public int? MetodoPagoId { get; set; }

        public string? MetodoPagoNombre { get; set; }
    }
}
