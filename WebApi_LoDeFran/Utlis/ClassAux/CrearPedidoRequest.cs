namespace WebApi_LoDeFran.Utlis.ClassAux
{
    public class CrearPedidoRequest
    {
        public int? ClienteId { get; set; }
        public int TipoPedidoId { get; set; }
        public int? UsuarioId { get; set; }
        public int? MesaId { get; set; }
        //public int? PromocionId { get; set; }
        //public decimal? MontoDescuento { get; set; }
        //public decimal? TotalSinDescuento { get; set; }
        //public string? Notas { get; set; }
    }
}
