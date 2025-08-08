using System;
using System.Collections.Generic;

namespace WebApi_LoDeFran.Models;

public partial class PromocionDia
{
    public int PromocionId { get; set; }

    public int DiaId { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public virtual Dia Dia { get; set; } = null!;

    public virtual Promocione Promocion { get; set; } = null!;
}
