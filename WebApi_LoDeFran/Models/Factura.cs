using System;
using System.Collections.Generic;

namespace WebApi_LoDeFran.Models;

public partial class Factura
{
    public int Id { get; set; }

    public int PedidoId { get; set; }

    public decimal Total { get; set; }

    public DateTime? FechaEmision { get; set; }

    public string Estado { get; set; } = null!;

    public int? ClienteId { get; set; }

    public int MetodoPagoId { get; set; }

    public virtual Cliente? Cliente { get; set; }

    public virtual MetodoPago MetodoPago { get; set; } = null!;

    public virtual Pedido Pedido { get; set; } = null!;
}
