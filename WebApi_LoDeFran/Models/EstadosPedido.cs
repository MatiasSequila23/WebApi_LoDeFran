using System;
using System.Collections.Generic;

namespace WebApi_LoDeFran.Models;

public partial class EstadosPedido
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
