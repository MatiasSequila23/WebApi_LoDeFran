using System;
using System.Collections.Generic;

namespace WebApi_LoDeFran.Models;

public partial class Pedido
{
    public int Id { get; set; }

    public int? ClienteId { get; set; }

    public int UsuarioId { get; set; }

    public DateTime? FechaPedido { get; set; }

    public decimal? Total { get; set; }

    public DateTime? FechaEntrega { get; set; }

    public int EstadoId { get; set; }

    public int? CategoriaId { get; set; }

    public virtual Cliente? Cliente { get; set; }

    public virtual ICollection<DetallesPedido> DetallesPedidos { get; set; } = new List<DetallesPedido>();

    public virtual EstadosPedido Estado { get; set; } = null!;

    public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();
}
