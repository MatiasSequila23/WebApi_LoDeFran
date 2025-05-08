using System;

namespace WebApi_LoDeFran.ViewModels
{
    public class ProveedorViewModel
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string? Telefono { get; set; }

        public string? Direccion { get; set; }

        public string? Email { get; set; }
    }
}
