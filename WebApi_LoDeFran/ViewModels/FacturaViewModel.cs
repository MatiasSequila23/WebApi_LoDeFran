namespace WebApi_LoDeFran.ViewModels
{
    public class FacturaViewModel
    {
        public int Id { get; set; }

        public int PedidoId { get; set; }
        public decimal Total { get; set; }

        public DateTime? FechaEmision { get; set; }

        public int? ClienteId { get; set; }
        public string? ClienteNombre { get; set; }

        public int MetodoPagoId { get; set; }
        public string? MetodoPagoNombre { get; set; }

        public int UsuarioId { get; set; }
        public string? UsuarioNombre { get; set; }

        public decimal? DescuentoAplicado { get; set; }

        public string? NumeroFactura { get; set; }
        public string? Observaciones { get; set; }
        public string? TipoFactura { get; set; }
        public string? CuitCliente { get; set; }

        public int? CajaId { get; set; }
        public string? CajaDescripcion { get; set; }

        public int EstadoId { get; set; }
        public string EstadoNombre { get; set; } = null!;
    }
}
