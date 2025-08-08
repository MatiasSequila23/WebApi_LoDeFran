using System;
using System.Collections.Generic;

namespace WebApi_LoDeFran.Models;

public partial class Combo
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public decimal Precio { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public int? Stock { get; set; }

    public int? EstadoId { get; set; }

    public virtual ICollection<CombosItem> CombosItems { get; set; } = new List<CombosItem>();

    public virtual ICollection<PedidoCombo> PedidoCombos { get; set; } = new List<PedidoCombo>();
}
