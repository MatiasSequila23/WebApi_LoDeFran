using System.ComponentModel.DataAnnotations;
namespace WebApi_LoDeFran.ViewModels
{
    public class ComboViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; } = null!;

        public string? Descripcion { get; set; }

        [Required]
        [Range(0.01, 999999.99)]
        public decimal Precio { get; set; }

        public DateTime? FechaCreacion { get; set; }
        public int? Stock { get; set; }

        public int? EstadoId { get; set; }

        // Lista de productos que componen el combo
        public List<ComboItemViewModel> Items { get; set; } = new();
    }
}
