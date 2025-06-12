namespace WebApi_LoDeFran.ViewModels
{
    public class MesaViewModel
    {
        public int Id { get; set; }
        public int Numero { get; set; }
        public int Capacidad { get; set; }
        public string Estado { get; set; } = null!;
        public int? EstadoId { get; set; }
        public string? EstadoNombre { get; set; }
        public int IdPiso { get; set; }                
        public string? PisoNombre { get; set; }
    }
}
