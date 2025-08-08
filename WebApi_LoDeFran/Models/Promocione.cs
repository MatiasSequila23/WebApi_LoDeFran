using System;
using System.Collections.Generic;

namespace WebApi_LoDeFran.Models;

public partial class Promocione
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public decimal? ValorDescuento { get; set; }

    public DateTime? FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }

    public decimal? MontoMinimo { get; set; }

    public int? EstadoId { get; set; }

    public int AplicacionId { get; set; }

    public int TipoDescuentoId { get; set; }

    public int? TipoPromocionId { get; set; }

    public virtual PromocionesAplicacione Aplicacion { get; set; } = null!;

    public virtual EstadosPromocione? Estado { get; set; }

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

    public virtual ICollection<PromocionDia> PromocionDia { get; set; } = new List<PromocionDia>();

    public virtual TipoDescuento TipoDescuento { get; set; } = null!;

    public virtual TipoPromocion? TipoPromocion { get; set; }
}
