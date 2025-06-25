namespace WebApi_LoDeFran.ViewModels
{
    public class CajaViewModel
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }

        public string? NombreUsuario { get; set; }  // Opcional para mostrar en pantalla

        public DateTime FechaApertura { get; set; }

        public DateTime? FechaCierre { get; set; }

        public decimal MontoInicial { get; set; }

        public decimal? MontoFinal { get; set; }

        public string Estado { get; set; } = null!;

        public List<MovimientoCajaViewModel> Movimientos { get; set; } = new();
    }
}
