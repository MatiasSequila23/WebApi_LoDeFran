using System;
using System.Collections.Generic;

namespace WebApi_LoDeFran.Models;

public partial class InsumosProducto
{
    public int ProductoId { get; set; }

    public int InsumoId { get; set; }

    public decimal? Cantidad { get; set; }

    public virtual Insumo Insumo { get; set; } = null!;

    public virtual Producto Producto { get; set; } = null!;
}
