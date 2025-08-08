namespace WebApi_LoDeFran.Utlis.Dto
{
    public class FacturaDto
    {
        public int PedidoId { get; set; }
        public decimal Total { get; set; }
        public int? ClienteId { get; set; }
        public int MetodoPagoId { get; set; }
        public int UsuarioId { get; set; }
        public decimal? DescuentoAplicado { get; set; }
        public string? Observaciones { get; set; }
        public string? TipoFactura { get; set; }
        public string? CuitCliente { get; set; }
        public int? CajaId { get; set; }
    }
}
