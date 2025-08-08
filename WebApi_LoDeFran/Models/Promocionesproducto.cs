using System;
using System.Collections.Generic;

namespace WebApi_LoDeFran.Models;

public partial class Promocionesproducto
{
    public int Id { get; set; }

    public int PromocionId { get; set; }

    public int ProductoId { get; set; }

    public virtual Producto Producto { get; set; } = null!;

    public virtual Promocione Promocion { get; set; } = null!;
}
