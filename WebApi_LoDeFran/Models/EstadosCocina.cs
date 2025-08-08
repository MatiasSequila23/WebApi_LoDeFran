using System;
using System.Collections.Generic;

namespace WebApi_LoDeFran.Models;

public partial class EstadosCocina
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<DetallesPedido> DetallesPedidos { get; set; } = new List<DetallesPedido>();
}
