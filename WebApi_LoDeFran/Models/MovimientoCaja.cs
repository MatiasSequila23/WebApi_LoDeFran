using System;
using System.Collections.Generic;

namespace WebApi_LoDeFran.Models;

public partial class MovimientoCaja
{
    public int Id { get; set; }

    public int CajaId { get; set; }

    public DateTime FechaMovimiento { get; set; }

    public string TipoMovimiento { get; set; } = null!;

    public decimal Monto { get; set; }

    public string? Descripcion { get; set; }

    public int? MetodoPagoId { get; set; }

    public virtual Caja Caja { get; set; } = null!;

    public virtual MetodoPago? MetodoPago { get; set; }
}
