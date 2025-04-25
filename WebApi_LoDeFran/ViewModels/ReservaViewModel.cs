namespace WebApi_LoDeFran.ViewModels
{
    public class ReservaViewModel
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public int MesaId { get; set; }
        public DateTime FechaHora { get; set; }
        public string Estado { get; set; } = null!;
        public string ClienteNombre { get; set; } = null!;
        public string MesaNumero { get; set; } = null!;
    }
}
