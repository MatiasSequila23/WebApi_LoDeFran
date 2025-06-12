using System;
using System.Collections.Generic;

namespace WebApi_LoDeFran.Models;

public partial class Mesa
{
    public int Id { get; set; }

    public int Numero { get; set; }

    public int Capacidad { get; set; }

    public int IdPiso { get; set; }

    public int IdEstado { get; set; }

    public virtual EstadosMesa IdEstadoNavigation { get; set; } = null!;

    public virtual Piso IdPisoNavigation { get; set; } = null!;

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
