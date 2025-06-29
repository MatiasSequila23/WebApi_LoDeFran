using System;
using System.Collections.Generic;

namespace WebApi_LoDeFran.Models;

public partial class MotivosMovimiento
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int TipoMovimientoId { get; set; }

    public virtual ICollection<MovimientoCaja> MovimientoCajas { get; set; } = new List<MovimientoCaja>();

    public virtual TiposMovimiento TipoMovimiento { get; set; } = null!;
}
