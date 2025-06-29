namespace WebApi_LoDeFran.ViewModels
{
    public class MovimientoCajaViewModel
    {
        public int Id { get; set; }
        public int CajaId { get; set; }
        public DateTime FechaMovimiento { get; set; }
        public decimal Monto { get; set; }
        public string? Descripcion { get; set; }

        public int? MetodoPagoId { get; set; }
        public string? MetodoPagoNombre { get; set; }

        public int? MotivoMovimientoId { get; set; }
        public string? MotivoNombre { get; set; }
        public string? TipoMovimientoNombre { get; set; } // Egreso / Ingreso
    }

}
