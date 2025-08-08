namespace WebApi_LoDeFran.ViewModels
{
    public class ClienteViewModel
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public string? Apellido { get; set; }

        public string? Email { get; set; }

        public string? Telefono { get; set; }

        public int? PuntosFidelidad { get; set; }

        public DateTime? FechaCreacion { get; set; }

        public string? Calle { get; set; }

        public string? Altura { get; set; }

        public string? Piso { get; set; }

        public int? CalleId { get; set; }

        // Relación con Calle (opcional, para mostrar nombre)
        public string? CalleNomMapa { get; set; } // opcional para mostrar

        public bool EsVip { get; set; }

        public DateOnly? FechaNacimiento { get; set; }
    }
}
