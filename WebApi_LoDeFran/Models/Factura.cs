using System;
using System.Collections.Generic;

namespace WebApi_LoDeFran.Models;

public partial class Factura
{
    public int Id { get; set; }

    public int PedidoId { get; set; }

    public decimal Total { get; set; }

    public DateTime? FechaEmision { get; set; }

    public int? ClienteId { get; set; }

    public int MetodoPagoId { get; set; }

    public int UsuarioId { get; set; }

    public decimal? DescuentoAplicado { get; set; }

    public string? NumeroFactura { get; set; }

    public string? Observaciones { get; set; }

    public string? TipoFactura { get; set; }

    public string? CuitCliente { get; set; }

    public int? CajaId { get; set; }

    public int EstadoId { get; set; }

    public virtual Caja? Caja { get; set; }

    public virtual Cliente? Cliente { get; set; }

    public virtual EstadosFactura Estado { get; set; } = null!;

    public virtual MetodoPago MetodoPago { get; set; } = null!;

    public virtual Pedido Pedido { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
